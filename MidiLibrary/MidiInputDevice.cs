using System.Runtime.InteropServices;
using System.Text;

using Midi.MidiMessages;
namespace Midi;

/// <summary>
/// Represents the delegate for the Input event
/// </summary>
/// <param name="sender">The sender</param>
/// <param name="args">The arguments</param>
public delegate void MidiInputEventHandler(object sender, MidiInputEventArgs args);

/// <summary>
/// Represents the arguments to the Input event
/// </summary>
/// <param name="TimeStamp"> The timestamp of the message </param>
/// <param name="Message"> The message </param>
public record MidiInputEventArgs(TimeSpan TimeStamp, MidiMessage Message);

/// <summary>
/// Represents a MIDI input device
/// </summary>
public sealed partial class MidiInputDevice : MidiDevice
{
    #region Win32

    private const int CALLBACK_FUNCTION = 196608;
    private const int MIM_OPEN = 961;
    private const int MIM_CLOSE = 962;
    private const int MIM_DATA = 963;
    private const int MIM_LONGDATA = 964;
    private const int MIM_ERROR = 965;
    private const int MIM_LONGERROR = 966;
    private const int MIM_MOREDATA = 972;
    private const int MHDR_DONE = 1;
    private const int MHDR_PREPARED = 2;
    private delegate void MidiInProc(nint handle, int wMsg, int dwInstance, int dwParam1, int dwParam2);
    [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
    private static extern int midiInGetErrorText(int errCode, StringBuilder message, int sizeOfMessage);
    [DllImport("winmm.dll")]
    private static extern int midiInGetDevCaps(int deviceIndex, ref MIDIINCAPS caps, int sizeOfMidiInCaps);
    [DllImport("winmm.dll")]
    private static extern int midiInOpen(out nint refHandle, int uDeviceID, MidiInProc dwCallback, int dwInstance, int dwFlags);
    [DllImport("winmm.dll")]
    private static extern int midiInClose(nint handle);
    [DllImport("winmm.dll")]
    private static extern int midiInStart(nint handle);
    [DllImport("winmm.dll")]
    private static extern int midiInStop(nint handle);
    [DllImport("winmm.dll")]
    private static extern int midiInReset(nint handle);
    [DllImport("winmm.dll")]
    private static extern int midiInAddBuffer(nint handle, ref MIDIHDR lpMidiHeader, int wSize);
    [DllImport("winmm.dll")]
    private static extern int midiInPrepareHeader(nint handle, ref MIDIHDR lpMidiHeader, int wSize);
    [DllImport("winmm.dll")]
    private static extern int midiInUnprepareHeader(nint handle, ref MIDIHDR lpMidiHeader, int wSize);
    [DllImport("Kernel32.dll")]
    private static extern void GetSystemTimePreciseAsFileTime(out long filetime);
    [StructLayout(LayoutKind.Sequential)]
    private struct MIDIINCAPS
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;     // MMVERSION

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;

        public uint dwSupport;
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct MIDIHDR
    {
        public nint lpData;          // offset  0- 3
        public uint dwBufferLength;  // offset  4- 7
        public uint dwBytesRecorded; // offset  8-11
        public nint dwUser;          // offset 12-15
        public uint dwFlags;         // offset 16-19
        public nint lpNext;          // offset 20-23
        public nint reserved;        // offset 24-27
        public uint dwOffset;        // offset 28-31
        public nint dwReserved0;
        public nint dwReserved1;
        public nint dwReserved2;
        public nint dwReserved3;
        public nint dwReserved4;
        public nint dwReserved5;
        public nint dwReserved6;
        public nint dwReserved7;
    }

    #endregion Win32

    private readonly MidiInProc _inCallback;
    private readonly int _index;
    private readonly object _recordingTrack0Lock = new();
    private MIDIINCAPS _caps;
    private nint _handle;
    private MIDIHDR _inHeader;
    private nint _buffer;
    private int _microTempo;
    private int _timeBase;
    private double _tempoSynchMininumTempo;
    private long _timingTimestamp;

    // must be an int to work with interlocked
    // 0=false, non-zero = true
    private int _tempoSynchEnabled;

    // indicates the frequency the tempo
    // can change given the sync signal
    // in system ticks
    private long _tempoSyncFrequency;

    // for tracking the above
    // reports the last time the
    // tempo was changed in
    // system ticks
    private long _tempoSyncTimestamp;

    // must be an int for Interlocked nonzero=true, zero=false
    //int _tapTempoEnabled;
    // the system timestamp for the last recorded message
    private long _recordingLastTimestamp;

    private MidiSequence _recordingTrack0;
    private MidiSequence _recordingSequence;
    private int _recordingPos;
    private MidiInputDeviceState _state;

    internal MidiInputDevice(int deviceIndex)
    {
        if (0 > deviceIndex)
            throw new ArgumentOutOfRangeException(nameof(deviceIndex));
        _handle = nint.Zero;
        _inCallback = new MidiInProc(_MidiInProc);
        _index = deviceIndex;
        _state = MidiInputDeviceState.Closed;
        _recordingLastTimestamp = 0L;
        _recordingPos = 0;
        _recordingTrack0 = null!;
        _recordingSequence = null!;
        _microTempo = 500000; // 120BPM
        _timeBase = 24;
        _timingTimestamp = 0L;
        _tempoSynchMininumTempo = 50d;
        _tempoSynchEnabled = 0; // false
        _tempoSyncFrequency = 0L;
        _tempoSyncTimestamp = 0L;
        _CheckInResult(midiInGetDevCaps(deviceIndex, ref _caps, Marshal.SizeOf(typeof(MIDIINCAPS))));
    }

    /// <summary>
    /// Raised when the device is opened
    /// </summary>
    public event EventHandler? Opened;

    /// <summary>
    /// Raised when the device is closed
    /// </summary>
    public event EventHandler? Closed;

    /// <summary>
    /// Raised when the tempo changes
    /// </summary>
    public event EventHandler? TempoChanged;

    /// <summary>
    /// Raised when incoming messages occur
    /// </summary>
    public event MidiInputEventHandler? Input;

    /// <summary>
    /// Raised when incoming messages occur
    /// </summary>
    public event MidiInputEventHandler? Error;

    /// <summary>
    /// Indicates the state of the device
    /// </summary>
    public MidiInputDeviceState State => _state;

    /// <summary>
    /// Indicates whether the input device is open
    /// </summary>
    public override bool IsOpen => nint.Zero != _handle;

    /// <summary>
    /// Indicates the time base of the input used for recording
    /// </summary>
    public short TimeBase
    {
        get
        {
            if (nint.Zero == _handle)
                throw new InvalidOperationException("The device is closed.");
            return unchecked((short)_timeBase);
        }
        set
        {
            if (nint.Zero == _handle)
                throw new InvalidOperationException("The device is closed.");
            if (0L != _recordingLastTimestamp)
                throw new InvalidOperationException("Cannot change the time base while recording.");
            Interlocked.Exchange(ref _timeBase, value);
        }
    }

    /// <summary>
    /// Indicates the micro tempo used for recording
    /// </summary>
    public int MicroTempo
    {
        get
        {
            if (nint.Zero == _handle)
                throw new InvalidOperationException("The device is closed.");
            return _microTempo;
        }
        set
        {
            if (nint.Zero == _handle)
                throw new InvalidOperationException("The device is closed.");
            if (value != _microTempo)
            {
                // if it's recording, generate a tempo change meta event
                // and put it in the sequence
                if (0 != _recordingLastTimestamp)
                {
                    lock (_recordingTrack0Lock)
                    {
                        _recordingTrack0?.AddAbsoluteEvent(_recordingPos, new MidiMessageMetaTempo(value));
                    }
                }
                Interlocked.Exchange(ref _microTempo, value);
                Interlocked.Exchange(ref _tempoSyncTimestamp, PreciseUtcNowTicks);
                TempoChanged?.BeginInvoke(this, EventArgs.Empty, null, null);
            }
        }
    }

    /// <summary>
    /// Indicates the tempo used for recording
    /// </summary>
    public double Tempo
    {
        get => MidiUtility.MicroTempoToTempo(MicroTempo);
        set => MicroTempo = MidiUtility.TempoToMicroTempo(value);
    }

    /// <summary>
    /// Indicates whether or not the system should respond to attempts
    /// to synchronize the tempo using MIDI realtime time clock
    /// messages
    /// </summary>
    /// <remarks>This is somewhat inaccurate, as .NET's latency is too high for an accurate measurement?</remarks>
    public bool TempoSynchronizationEnabled
    {
        get => 0 != _tempoSynchEnabled;
        set
        {
            if (value && 0 == _tempoSynchEnabled)
                Interlocked.Exchange(ref _tempoSynchEnabled, 1);
            else if (!value && 0 != _tempoSynchEnabled)
                Interlocked.Exchange(ref _tempoSynchEnabled, 0);
        }
    }

    /// <summary>
    /// Indicates the quantization factor to use for tempo synchronization
    /// </summary>
    public TimeSpan TempoSychronizationFrequency
    {
        get => new(_tempoSyncFrequency);
        set => Interlocked.Exchange(ref _tempoSyncFrequency, value.Ticks);
    }

    /// <summary>
    /// Indicates the minumum micro tempo for the tempo synchronization feature
    /// </summary>
    public int TempoSynchronizationMinimumMicroTempo
    {
        get => MidiUtility.TempoToMicroTempo(TempoSynchronizationMinimumTempo);
        set => MidiUtility.MicroTempoToTempo(value);
    }

    /// <summary>
    /// Indicates the minumum tempo for the tempo synchronization feature
    /// </summary>
    public double TempoSynchronizationMinimumTempo
    {
        get => _tempoSynchMininumTempo;
        set => Interlocked.Exchange(ref _tempoSynchMininumTempo, value);
    }

    /// <summary>
    /// Indicates the index of the input device
    /// </summary>
    public override int Index => _index;

    /// <summary>
    /// Indicates the name of the input device
    /// </summary>
    public override string Name => _caps.szPname;

    /// <summary>
    /// Indicates the version of the driver associated with the device
    /// </summary>
    public override Version Version => new(_caps.vDriverVersion >> 16, _caps.vDriverVersion & 0xFFFF);

    /// <summary>
    /// Indicates the product code for the device
    /// </summary>
    public override short ProductId => _caps.wPid;

    /// <summary>
    /// Indicates the manufacturer code for the device
    /// </summary>
    public override short ManufacturerId => _caps.wMid;

    private static long PreciseUtcNowTicks
    {
        get
        {
            GetSystemTimePreciseAsFileTime(out long filetime);
            return filetime + 504911232000000000;
        }
    }

    /// <summary>
    /// Opens the MIDI input device
    /// </summary>
    public override void Open()
    {
        Close();
        _CheckInResult(midiInOpen(out _handle, _index, _inCallback, 0, CALLBACK_FUNCTION));
        var sz = Marshal.SizeOf(typeof(MIDIHDR));
        _inHeader.dwBufferLength = _inHeader.dwBytesRecorded = 65536u;
        _inHeader.lpData = _buffer = Marshal.AllocHGlobal(65536);
        _CheckInResult(midiInPrepareHeader(_handle, ref _inHeader, sz));
        _CheckInResult(midiInAddBuffer(_handle, ref _inHeader, sz));
        _state = MidiInputDeviceState.Stopped;
    }

    /// <summary>
    /// Closes the MIDI input device
    /// </summary>
    public override void Close()
    {
        if (MidiInputDeviceState.Closed != _state)
        {
            if (MidiInputDeviceState.Started == _state)
                Stop();
            // flush any pending events
            Reset();
            var sz = Marshal.SizeOf(typeof(MIDIHDR));
            var ptr = _inHeader.lpData;
            // in case the header is being used:
            if (0 != _inHeader.dwFlags && MHDR_PREPARED != (_inHeader.dwFlags & MHDR_PREPARED))
                while ((_inHeader.dwFlags & MHDR_DONE) != MHDR_DONE)
                    Thread.Sleep(1);
            _CheckInResult(midiInUnprepareHeader(_handle, ref _inHeader, sz));
            _CheckInResult(midiInClose(_handle));
            Marshal.FreeHGlobal(ptr);
            _state = MidiInputDeviceState.Closed;
            Interlocked.Exchange(ref _timeBase, 24);
            Interlocked.Exchange(ref _microTempo, 500000);
        }
    }

    /// <summary>
    /// Starts the MIDI input device
    /// </summary>
    public void Start()
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The device is closed.");
        _CheckInResult(midiInStart(_handle));
        _state = MidiInputDeviceState.Started;
    }

    /// <summary>
    /// Stops the MIDI input device
    /// </summary>
    public void Stop()
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The device is closed.");
        _CheckInResult(midiInStop(_handle));
        _state = MidiInputDeviceState.Stopped;
    }

    /// <summary>
    /// Resets the MIDI input device
    /// </summary>
    public void Reset()
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The device is closed.");
        _CheckInResult(midiInReset(_handle));
    }

    /// <summary>
    /// Starts recording to a MIDI file
    /// </summary>
    /// <param name="waitForInput">True if recording should be deferred until MIDI input is recieved, otherwise false to start right away</param>
    public void StartRecording(bool waitForInput = false)
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The device is closed.");
        if (0 != _recordingLastTimestamp)
            throw new InvalidOperationException("The device is already recording.");
        Interlocked.Exchange(ref _recordingPos, 0);
        lock (_recordingTrack0Lock)
            _recordingTrack0 = new MidiSequence();

        Interlocked.Exchange(ref _recordingSequence, new MidiSequence());

        if (MidiInputDeviceState.Started != _state)
            Start();

        if (!waitForInput)
            Interlocked.Exchange(ref _recordingLastTimestamp, PreciseUtcNowTicks);
    }

    /// <summary>
    /// Ends the current recording session, returning a MIDI file
    /// </summary>
    /// <param name="trimRemainder">Indicates whether or not the silent remainder of the recording (if any) is trimmed</param>
    /// <returns>The MIDI file containing the recorded performance, or null if recording was never started.</returns>
    /// <remarks>The returned file is always a type 1 MIDI file at the stream's timebase, and following the stream's tempo. The file consists of two tracks. Track 0 is a meta track containing the tempo map, and the other track contains the performance data</remarks>
    public MidiFile? EndRecording(bool trimRemainder = false)
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The device is closed.");
        if (0 == _recordingLastTimestamp)
            return null;
        var result = new MidiFile(1, unchecked((short)_timeBase));
        var tb = _timeBase;
        var mt = _microTempo;
        var pos = _recordingPos;
        var ts = _recordingLastTimestamp;
        Interlocked.Exchange(ref _recordingLastTimestamp, 0);
        Interlocked.Exchange(ref _recordingPos, 0);
        lock (_recordingTrack0Lock)
        {
            result.Tracks.Add(_recordingTrack0);
            Interlocked.Exchange(ref _recordingTrack0, null!);
        }
        result.Tracks.Add(_recordingSequence);
        Interlocked.Exchange(ref _recordingSequence, null!);
        _ = new MidiSequence();
        int len;
        if (!trimRemainder)
        {
            // we need to compute the number of MIDI ticks since
            // _recordingLastTimestamp (last message received)
            // first recompute our timing
            var ticksusec = mt / (double)tb;
            var tickspertick = ticksusec / (TimeSpan.TicksPerMillisecond / 1000) * 100;
            // now convert the time difference to MIDI ticks
            var remst = unchecked((int)Math.Round((PreciseUtcNowTicks - ts) / tickspertick, MidpointRounding.AwayFromZero));
            // tack it on to the length
            len = pos + remst;
        }
        else
            len = result.Tracks[1].Length;
        //endTrack.Events.Add(new MidiEvent(len, new MidiMessageMetaEndOfTrack()));
        result.Tracks[0].AddAbsoluteEvent(len, new MidiMessageMetaEndOfTrack());// = MidiSequence.Merge(result.Tracks[0], endTrack);
        result.Tracks[1].AddAbsoluteEvent(len, new MidiMessageMetaEndOfTrack());//= MidiSequence.Merge(result.Tracks[1], endTrack);
        return result;
    }

    private static string _GetMidiInErrorMessage(int errorCode)
    {
        var result = new StringBuilder(256);
        _ = midiInGetErrorText(errorCode, result, result.Capacity);
        return result.ToString();
    }

    [System.Diagnostics.DebuggerNonUserCode()]
    private static void _CheckInResult(int errorCode)
    {
        if (0 != errorCode)
            throw new Exception(_GetMidiInErrorMessage(errorCode));
    }

    private void _MidiInProc(nint handle, int msg, int instance, int lparam, int wparam)
    {
        MidiMessage m;
        switch (msg)
        {
            case MIM_OPEN:
                Opened?.Invoke(this, EventArgs.Empty);
                break;
            case MIM_CLOSE:
                Closed?.Invoke(this, EventArgs.Empty);
                break;
            case MIM_DATA:
                if (0 != _tempoSynchEnabled && 0xF8 == (0xFF & lparam))
                {
                    if (0 != _timingTimestamp)
                    {
                        var dif = (PreciseUtcNowTicks - _timingTimestamp) * 24;
                        var tpm = TimeSpan.TicksPerMillisecond * 60000;
                        var newTempo = (tpm / (double)dif);
                        if (newTempo < _tempoSynchMininumTempo)
                            Interlocked.Exchange(ref _timingTimestamp, 0);
                        else
                        {
                            var timeNow = PreciseUtcNowTicks;

                            if (0L == _tempoSyncTimestamp || 0L == _tempoSyncFrequency || (timeNow - _tempoSyncTimestamp > _tempoSyncFrequency))
                            {
                                var tmp = Tempo;
                                var ta = (tmp + newTempo) / 2;
                                Tempo = ta;
                            }
                            Interlocked.Exchange(ref _timingTimestamp, timeNow);
                        }
                    }
                    else
                    {
                        var timeNow = PreciseUtcNowTicks;
                        Interlocked.Exchange(ref _timingTimestamp, timeNow);
                    }
                }
                else
                {
                    m = MidiUtility.UnpackMessage(lparam);
                    _ProcessRecording(m);
                    Input?.Invoke(this, new MidiInputEventArgs(new TimeSpan(0, 0, 0, 0, wparam), m));
                }
                break;
            case MIM_ERROR:
                Error?.Invoke(this, new MidiInputEventArgs(new TimeSpan(0, 0, 0, 0, wparam), MidiUtility.UnpackMessage(lparam)));
                break;
            case MIM_LONGERROR:

            case MIM_LONGDATA:
                // TODO: Semi tested
                var hdr = (MIDIHDR)Marshal.PtrToStructure(new nint(lparam), typeof(MIDIHDR))!;
                // for some reason we're getting bogus MIDIHDRs from lparam sometimes.
                // hopefully this catches it:
                if (0 == hdr.dwBytesRecorded || 65536 < hdr.dwBytesRecorded)
                    return; // no message
                            // this code assumes it's a sysex message but I should probably check it.
                if (nint.Zero != hdr.lpData)
                {
                    var status = Marshal.ReadByte(hdr.lpData, 0);
                    var payload = new byte[hdr.dwBytesRecorded - 1];
                    Marshal.Copy(new nint(checked((int)hdr.lpData) + 1), payload, 0, payload.Length);
                    m = new MidiMessageSysex(payload);
                    var sz = Marshal.SizeOf(typeof(MIDIHDR));
                    _inHeader.dwBufferLength = _inHeader.dwBytesRecorded = 65536u;
                    _inHeader.lpData = _buffer;
                    _CheckInResult(midiInPrepareHeader(_handle, ref _inHeader, sz));
                    _CheckInResult(midiInAddBuffer(_handle, ref _inHeader, sz));
                    _ProcessRecording(m);
                    if (MIM_LONGDATA == msg)
                        Input?.Invoke(this, new MidiInputEventArgs(new TimeSpan(0, 0, 0, 0, wparam), m));
                    else
                        Error?.Invoke(this, new MidiInputEventArgs(new TimeSpan(0, 0, 0, 0, wparam), m));
                }
                break;
            case MIM_MOREDATA:
                break;
            default:
                break;
        }
    }

    private void _ProcessRecording(MidiMessage msg)
    {
        var mt = _microTempo;
        var tb = _timeBase;
        var rst = _recordingLastTimestamp;
        MidiSequence? t0 = null;
        lock (_recordingTrack0Lock)
            t0 = _recordingTrack0;
        var rs = _recordingSequence;
        if (null != _recordingSequence)
        {
            lock (_recordingTrack0Lock)
            {
                if (0 == t0.Events.Count)
                {
                    t0.Events.Add(new MidiEvent(0, new MidiMessageMetaTempo(mt)));
                }
            }
            // recompute our timing based on current microTempo and timeBase
            var ticksusec = mt / (double)tb;
            var tickspertick = ticksusec / (TimeSpan.TicksPerMillisecond / 1000) * 100;
            var timeNow = PreciseUtcNowTicks;
            // initialize start ticks with the current time in ticks
            if (0 == rst)
            {
                var r = timeNow;
                Interlocked.Exchange(ref _recordingLastTimestamp, r);
            }
            var span = timeNow - _recordingLastTimestamp;
            Interlocked.Exchange(ref _recordingLastTimestamp, timeNow);
            // compute our current MIDI ticks
            var midiTicks = (int)Math.Round(span / tickspertick);
            // HACK: technically the sequence isn't threadsafe but as long as this event
            // is not reentrant and the MidiSequence isn't touched outside this it should
            // be fine
            rs.Events.Add(new MidiEvent(midiTicks, msg));
            // update the recording position
            Interlocked.Exchange(ref _recordingPos, _recordingPos + midiTicks);
        }
    }
}
