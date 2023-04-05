using Midi.MidiMessages;
using System.Runtime.InteropServices;
using System.Text;

namespace Midi;

/// <summary>
/// Represents a MIDI stream
/// </summary>
public class MidiStream : MidiOutputDevice
{
    #region Win32

    private const int MEVT_TEMPO = 0x01;

    private const int MEVT_NOP = 0x02;

    private const int CALLBACK_FUNCTION = 196608;

    private const int MOM_OPEN = 0x3C7;

    private const int MOM_CLOSE = 0x3C8;

    private const int MOM_DONE = 0x3C9;

    private const int TIME_MS = 0x0001;

    private const int TIME_BYTES = 0x0004;

    private const int TIME_SMPTE = 0x0008;

    private const int TIME_MIDI = 0x0010;

    private const int TIME_TICKS = 0x0020;

    private const int MIDIPROP_SET = unchecked((int)0x80000000);

    private const int MIDIPROP_GET = 0x40000000;

    private const int MIDIPROP_TIMEDIV = 1;

    private const int MIDIPROP_TEMPO = 2;

    private const int MEVT_F_LONG = unchecked((int)0x80000000);

    private const int TIME_ONESHOT = 0;

    private static readonly int MIDIHDR_SIZE = Marshal.SizeOf(typeof(MIDIHDR));

    private static readonly int MIDIEVENT_SIZE = Marshal.SizeOf(typeof(MIDIEVENT));

    private delegate void MidiOutProc(nint handle, int msg, int instance, nint param1, nint param2);

    private delegate void TimerProc(nint handle, int msg, int instance, nint param1, nint param2);

    [DllImport("winmm.dll")]
    private static extern int midiStreamOpen(ref nint handle, ref int deviceID, int cMidi,
        MidiOutProc proc, int instance, int flags);

    [DllImport("winmm.dll")]
    private static extern int midiStreamProperty(nint handle, ref MIDIPROPTEMPO tempo, int dwProperty);

    [DllImport("winmm.dll")]
    private static extern int midiStreamProperty(nint handle, ref MIDIPROPTIMEDIV timeDiv, int dwProperty);

    [DllImport("winmm.dll")]
    private static extern int midiStreamClose(nint handle);

    [DllImport("winmm.dll")]
    private static extern int midiStreamRestart(nint handle);

    [DllImport("winmm.dll")]
    private static extern int midiStreamPause(nint handle);

    [DllImport("winmm.dll")]
    private static extern int midiStreamStop(nint handle);

    [DllImport("winmm.dll")]
    private static extern int midiStreamOut(nint handle, nint lpMidiOutHdr, int uSize);

    [DllImport("winmm.dll")]
    private static extern int midiOutPrepareHeader(nint hMidiOut, ref MIDIHDR lpMidiOutHdr, int uSize);

    [DllImport("winmm.dll")]
    private static extern int midiOutPrepareHeader(nint hMidiOut, nint lpMidiOutHdr, int uSize);

    [DllImport("winmm.dll")]
    private static extern int midiOutUnprepareHeader(nint hMidiOut, ref MIDIHDR lpMidiOutHdr, int uSize);

    [DllImport("winmm.dll")]
    private static extern int midiOutUnprepareHeader(nint hMidiOut, nint lpMidiOutHdr, int uSize);

    [DllImport("winmm.dll")]
    private static extern int midiStreamPosition(nint handle, ref MMTIME lpMMTime, int uSize);

    [DllImport("winmm.dll")]
    private static extern int midiOutShortMsg(nint handle, int message);

    [DllImport("winmm.dll")]
    private static extern int midiOutLongMsg(nint hMidiOut, ref MIDIHDR lpMidiOutHdr, int uSize);

    [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
    private static extern int midiOutGetErrorText(int errCode,
       StringBuilder message, int sizeOfMessage);

    [DllImport("winmm.dll")]
    private static extern nint timeSetEvent(int delay, int resolution, TimerProc handler, nint user, int eventType);

    [DllImport("winmm.dll")]
    private static extern int timeKillEvent(nint handle);

    [DllImport("winmm.dll")]
    private static extern int timeBeginPeriod(int msec);

    [DllImport("winmm.dll")]
    private static extern int timeEndPeriod(int msec);

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

    [StructLayout(LayoutKind.Sequential)]
    private struct MIDIPROPTIMEDIV
    {
        public int cbStruct;
        public int dwTimeDiv;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MIDIPROPTEMPO
    {
        public int cbStruct;
        public int dwTempo;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct MMTIME
    {
        [FieldOffset(0)] public int wType;
        [FieldOffset(4)] public int ms;
        [FieldOffset(4)] public int sample;
        [FieldOffset(4)] public int cb;
        [FieldOffset(4)] public int ticks;
        [FieldOffset(4)] public byte smpteHour;
        [FieldOffset(5)] public byte smpteMin;
        [FieldOffset(6)] public byte smpteSec;
        [FieldOffset(7)] public byte smpteFrame;
        [FieldOffset(8)] public byte smpteFps;
        [FieldOffset(9)] public byte smpteDummy;
        [FieldOffset(10)] public byte pad0;
        [FieldOffset(11)] public byte pad1;
        [FieldOffset(4)] public int midiSongPtrPos;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MIDIEVENT
    {
        public int dwDeltaTime;
        public int dwStreamId;
        public int dwEvent;
    }

    #endregion Win32

    private static readonly int MAX_EVENTBLOCK_SIZE = 65536 - MIDIHDR_SIZE;

    private readonly MidiOutProc _outCallback;
    private readonly TimerProc _timerCallback;
    private nint _timerHandle;
    private int _sendQueuePosition;
    private int _tempoSyncMessageCount;
    private int _tempoSyncMessagesSentCount;

    // must be an int to use interlocked
    // 0=false, nonzero = true
    private int _tempoSyncEnabled;

    private List<MidiEvent>? _sendQueue;
    private MidiStreamState _state = MidiStreamState.Closed;

    internal MidiStream(int index) : base(index)
    {
        _sendQueue = null;
        _sendQueuePosition = 0;
        _outCallback = new MidiOutProc(_MidiOutProc);
        _timerCallback = new TimerProc(_TimerProc);
        _tempoSyncEnabled = 0;
        _tempoSyncMessageCount = 100;
        _tempoSyncMessagesSentCount = 0;
    }

    /// <summary>
    /// Destroys this instance
    /// </summary>
    ~MidiStream()
    {
        Close();
    }

    /// <summary>
    /// Raised when a Send() operation has completed. This only applies to sending MidiEvent items
    /// </summary>
    public event EventHandler? SendComplete;

    /// <summary>
    /// Indicates the state of the MIDI stream
    /// </summary>
    public MidiStreamState State
    { get { return _state; } }

    /// <summary>
    /// Indicates whether or not the stream attempts to synchronize the remote device's tempo
    /// </summary>
    public bool TempoSynchronizationEnabled
    {
        get
        {
            return 0 != _tempoSyncEnabled;
        }
        set
        {
            if (value)
            {
                if (MidiStreamState.Started == _state)
                {
                    var tmp = Tempo;
                    var spb = 60 / tmp;
                    var ms = unchecked((int)(Math.Round((1000 * spb) / 24)));
                    _RestartTimer(ms);
                }
                Interlocked.Exchange(ref _tempoSyncEnabled, 1);
                return;
            }
            Interlocked.Exchange(ref _tempoSyncEnabled, 0);
            Interlocked.Exchange(ref _tempoSyncMessagesSentCount, 0);
            _DisposeTimer();
        }
    }

    /// <summary>
    /// Indicates the number of time clock sync messages to send when the tempo is changed. 0 indicates continuous synchronization
    /// </summary>
    public int TempoSynchronizationMessageCount
    {
        get
        {
            return _tempoSyncMessageCount;
        }
        set
        {
            Interlocked.Exchange(ref _tempoSyncMessageCount, value);
        }
    }

    /// <summary>
    /// Indicates the position in ticks
    /// </summary>
    public int PositionTicks
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            switch (_state)
            {
                case MidiStreamState.Started:
                case MidiStreamState.Paused:
                    MMTIME mm = new()
                    {
                        wType = TIME_TICKS
                    };
                    _CheckOutResult(midiStreamPosition(Handle, ref mm, Marshal.SizeOf(typeof(MMTIME))));
                    if (TIME_TICKS != mm.wType)
                        throw new NotSupportedException("The position format is not supported.");
                    return mm.ticks;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Indicates the position in milliseconds
    /// </summary>
    public int PositionMilliseconds
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            switch (_state)
            {
                case MidiStreamState.Started:
                case MidiStreamState.Paused:
                    MMTIME mm = new()
                    {
                        wType = TIME_MS
                    };
                    _CheckOutResult(midiStreamPosition(Handle, ref mm, Marshal.SizeOf(typeof(MMTIME))));
                    if (TIME_MS != mm.wType)
                        throw new NotSupportedException("The position format is not supported.");
                    return mm.ms;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Indicates the song pointer position
    /// </summary>
    public int PositionSongPointer
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            switch (_state)
            {
                case MidiStreamState.Started:
                case MidiStreamState.Paused:
                    MMTIME mm = new()
                    {
                        wType = TIME_MIDI
                    };
                    _CheckOutResult(midiStreamPosition(Handle, ref mm, Marshal.SizeOf(typeof(MMTIME))));
                    if (TIME_MIDI != mm.wType)
                        throw new NotSupportedException("The position format is not supported.");
                    return mm.midiSongPtrPos;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Indicates the position in bytes
    /// </summary>
    public int PositionBytes
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            switch (_state)
            {
                case MidiStreamState.Started:
                case MidiStreamState.Paused:
                    MMTIME mm = new()
                    {
                        wType = TIME_BYTES
                    };
                    _CheckOutResult(midiStreamPosition(Handle, ref mm, Marshal.SizeOf(typeof(MMTIME))));
                    if (TIME_BYTES != mm.wType)
                        throw new NotSupportedException("The position format is not supported.");
                    return mm.cb;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Indicates the position in SMPTE format
    /// </summary>
    public MidiSmpteTime PositionSmpte
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            switch (_state)
            {
                case MidiStreamState.Started:
                case MidiStreamState.Paused:
                    MMTIME mm = new()
                    {
                        wType = TIME_SMPTE
                    };
                    _CheckOutResult(midiStreamPosition(Handle, ref mm, Marshal.SizeOf(typeof(MMTIME))));
                    if (TIME_SMPTE != mm.wType)
                        throw new NotSupportedException("The position format is not supported.");
                    return new MidiSmpteTime(new TimeSpan(0, mm.smpteHour, mm.smpteMin, mm.smpteSec, 0), mm.smpteFrame, mm.smpteFps);
                default:
                    return default;
            }
        }
    }

    /// <summary>
    /// Indicates the MicroTempo of the stream
    /// </summary>
    public int MicroTempo
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            var t = new MIDIPROPTEMPO
            {
                cbStruct = Marshal.SizeOf(typeof(MIDIPROPTEMPO))
            };
            _CheckOutResult(midiStreamProperty(Handle, ref t, MIDIPROP_GET | MIDIPROP_TEMPO));
            return unchecked(t.dwTempo);
        }
        set
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            Interlocked.Exchange(ref _tempoSyncMessagesSentCount, 0);
            var t = new MIDIPROPTEMPO
            {
                cbStruct = Marshal.SizeOf(typeof(MIDIPROPTEMPO)),
                dwTempo = value
            };
            _CheckOutResult(midiStreamProperty(Handle, ref t, MIDIPROP_SET | MIDIPROP_TEMPO));
        }
    }

    /// <summary>
    /// Indicates the Tempo of the stream
    /// </summary>
    public double Tempo
    {
        get
        {
            return MidiUtility.MicroTempoToTempo(MicroTempo);
        }
        set
        {
            MicroTempo = MidiUtility.TempoToMicroTempo(value);
        }
    }

    /// <summary>
    /// Indicates the TimeBase of the stream
    /// </summary>
    public short TimeBase
    {
        get
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            var tb = new MIDIPROPTIMEDIV
            {
                cbStruct = Marshal.SizeOf(typeof(MIDIPROPTIMEDIV))
            };
            _CheckOutResult(midiStreamProperty(Handle, ref tb, MIDIPROP_GET | MIDIPROP_TIMEDIV));
            return unchecked((short)tb.dwTimeDiv);
        }
        set
        {
            if (nint.Zero == Handle)
                throw new InvalidOperationException("The stream is closed.");
            var tb = new MIDIPROPTIMEDIV
            {
                cbStruct = Marshal.SizeOf(typeof(MIDIPROPTIMEDIV)),
                dwTimeDiv = value
            };
            _CheckOutResult(midiStreamProperty(Handle, ref tb, MIDIPROP_SET | MIDIPROP_TIMEDIV));
        }
    }

    /// <summary>
    /// Opens the stream
    /// </summary>
    public override void Open()
    {
        if (nint.Zero != Handle)
            throw new InvalidOperationException("The device is already open");
        var di = Index;
        var h = nint.Zero;
        Interlocked.Exchange(ref _sendQueue, null);
        Interlocked.Exchange(ref _sendQueuePosition, 0);
        _CheckOutResult(midiStreamOpen(ref h, ref di, 1, _outCallback, 0, CALLBACK_FUNCTION));
        Handle = h;
        _state = MidiStreamState.Paused;
    }

    /// <summary>
    /// Closes the stream
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816", Justification = "<挂起>")]
    public override void Close()
    {
        _DisposeTimer();
        if (nint.Zero != Handle)
        {
            Stop();
            Reset();
            _CheckOutResult(midiStreamClose(Handle));
            Handle = nint.Zero;
            GC.SuppressFinalize(this);
            Interlocked.Exchange(ref _sendQueue, null);
            Interlocked.Exchange(ref _sendQueuePosition, 0);
            _state = MidiStreamState.Closed;
        }
    }

    /// <summary>
    /// Sends MIDI events to the stream
    /// </summary>
    /// <param name="events">The events to send</param>
    public void Send(params MidiEvent[] events)
        => Send((IEnumerable<MidiEvent>)events);

    /// <summary>
    /// Sends a MIDI event to the stream
    /// </summary>
    /// <param name="events">The events to send</param>
    public void Send(IEnumerable<MidiEvent> events)
    {
        if (null != _sendQueue)
        {
            throw new InvalidOperationException("The stream is already sending");
        }

        var list = new List<MidiEvent>(128);
        // break out sysex messages into parts
        foreach (var @event in events)
        {
            if (@event.Message is null)
                continue;
            // sysex
            if (0xF0 == @event.Message.Status)
            {
                var data = ((MidiMessageSysex)@event.Message).Data;
                if (null == data)
                    return;
                if (254 < data.Length)
                {
                    var len = 254;
                    for (var i = 0; i < data.Length; i += len)
                    {
                        if (data.Length <= i + len)
                        {
                            len = data.Length - i;
                        }
                        var buf = new byte[len];
                        if (0 == i)
                        {
                            Array.Copy(data, 0, buf, 0, len);
                            list.Add(new MidiEvent(@event.Position, new MidiMessageSysex(buf)));
                        }
                        else
                        {
                            Array.Copy(data, i, buf, 0, len);
                            list.Add(new MidiEvent(@event.Position, new MidiMessageSysexPart(buf)));
                        }
                    }
                }
                else
                {
                    list.Add(@event);
                }
            }
            else
                list.Add(@event);
        }
        Interlocked.Exchange(ref _sendQueue, list);
        Interlocked.Exchange(ref _sendQueuePosition, 0);
        _SendBlock();
    }

    /// <summary>
    /// Sends events directly to the stream
    /// </summary>
    /// <param name="events">The events to send</param>
    public void SendDirect(params MidiEvent[] events)
        => SendDirect((IEnumerable<MidiEvent>)events);

    /// <summary>
    /// Sends events directly to the event queue without buffering
    /// </summary>
    /// <param name="events">The events to send</param>
    /// <remarks>The total size of the events must be less than 64kb</remarks>
    public void SendDirect(IEnumerable<MidiEvent> events)
    {
        if (null == events)
            throw new ArgumentNullException(nameof(events));
        if (nint.Zero == Handle)
            throw new InvalidOperationException("The stream is closed.");
        if (null != _sendQueue)
            throw new InvalidOperationException("The stream is already sending.");
        int blockSize = 0;
        nint headerPointer = Marshal.AllocHGlobal(MAX_EVENTBLOCK_SIZE + MIDIHDR_SIZE);
        try
        {
            nint eventPointer = new(headerPointer.ToInt64() + MIDIHDR_SIZE);
            var ofs = 0;
            var ptrOfs = 0;
            var hasEvents = false;
            foreach (var @event in events)
            {
                if (@event.Message is null)
                    continue;
                hasEvents = true;
                if (0xF0 != (@event.Message.Status & 0xF0))
                {
                    blockSize += MIDIEVENT_SIZE;
                    if (MAX_EVENTBLOCK_SIZE <= blockSize)
                        throw new ArgumentException("There are too many events in the event buffer - maximum size must be 64k", nameof(events));
                    var se = default(MIDIEVENT);
                    se.dwDeltaTime = @event.Position + ofs;
                    se.dwStreamId = 0;
                    se.dwEvent = MidiUtility.PackMessage(@event.Message);
                    Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                    ptrOfs += MIDIEVENT_SIZE;
                    ofs = 0;
                }
                else if (0xFF == @event.Message.Status)
                {
                    var mm = (MidiMessageMeta)@event.Message;
                    if (0x51 == mm.Data1) // tempo
                    {
                        blockSize += MIDIEVENT_SIZE;
                        if (MAX_EVENTBLOCK_SIZE <= blockSize)
                            throw new ArgumentException("There are too many events in the event buffer - maximum size must be 64k", nameof(events));

                        var se = default(MIDIEVENT);
                        se.dwDeltaTime = @event.Position + ofs;
                        se.dwStreamId = 0;
                        se.dwEvent = (mm.Data[0] << 16) | (mm.Data[1] << 8) | mm.Data[2] | (MEVT_TEMPO << 24);
                        Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                        ptrOfs += MIDIEVENT_SIZE;
                        ofs = 0;
                        // TODO: This signal is sent too early. It should really wait until after the
                        // MEVT_TEMPO message is processed by the driver, but i have no easy way to
                        // do that. All we can do is hope, here
                        Interlocked.Exchange(ref _tempoSyncMessagesSentCount, 0);
                    }
                    else if (0x2f == mm.Data1) // end track
                    {
                        blockSize += MIDIEVENT_SIZE;
                        if (MAX_EVENTBLOCK_SIZE <= blockSize)
                            throw new ArgumentException("There are too many events in the event buffer - maximum size must be 64k", nameof(events));

                        // add a NOP message to it just to pad our output in case we're looping
                        var se = default(MIDIEVENT);
                        se.dwDeltaTime = @event.Position + ofs;
                        se.dwStreamId = 0;
                        se.dwEvent = (MEVT_NOP << 24);
                        Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                        ptrOfs += MIDIEVENT_SIZE;
                        ofs = 0;
                    }
                    else
                        ofs = @event.Position;
                }
                else // sysex
                {
                    var msx = (MidiMessageSysex)@event.Message;
                    var dl = msx.Data.Length + 1;
                    if (0 != (dl % 4))
                    {
                        dl += 4 - (dl % 4);
                    }
                    blockSize += MIDIEVENT_SIZE + dl;
                    if (MAX_EVENTBLOCK_SIZE <= blockSize)
                        throw new ArgumentException("There are too many events in the event buffer - maximum size must be 64k", nameof(events));

                    var se = default(MIDIEVENT);
                    se.dwDeltaTime = @event.Position + ofs;
                    se.dwStreamId = 0;
                    se.dwEvent = MEVT_F_LONG | (msx.Data.Length + 1);
                    Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                    ptrOfs += MIDIEVENT_SIZE;
                    Marshal.WriteByte(new nint(ptrOfs + eventPointer.ToInt64()), msx.Status);
                    Marshal.Copy(msx.Data, 0, new nint(ptrOfs + eventPointer.ToInt64() + 1), msx.Data.Length);

                    ptrOfs += dl;
                    ofs = 0;
                }
            }
            if (hasEvents)
            {
                var header = default(MIDIHDR);
                header.lpData = eventPointer;
                header.dwBufferLength = header.dwBytesRecorded = unchecked((uint)blockSize);
                Marshal.StructureToPtr(header, headerPointer, false);
                _CheckOutResult(midiOutPrepareHeader(Handle, headerPointer, MIDIHDR_SIZE));
                _CheckOutResult(midiStreamOut(Handle, headerPointer, MIDIHDR_SIZE));
                headerPointer = nint.Zero;
            }
        }
        finally
        {
            if (nint.Zero != headerPointer)
                Marshal.FreeHGlobal(headerPointer);
        }
    }

    /// <summary>
    /// Starts the stream
    /// </summary>
    public void Start()
    {
        if (nint.Zero == Handle)
            throw new InvalidOperationException("The stream is closed.");
        switch (_state)
        {
            case MidiStreamState.Paused:
            case MidiStreamState.Stopped:

                var tmp = Tempo;
                var spb = 60 / tmp;
                var ms = unchecked((int)(Math.Round((1000 * spb) / 24)));
                Interlocked.Exchange(ref _tempoSyncMessagesSentCount, 0);
                _RestartTimer(ms);

                _CheckOutResult(midiStreamRestart(Handle));
                _state = MidiStreamState.Started;
                break;
        }
    }

    /// <summary>
    /// Stops the stream
    /// </summary>
    public void Stop()
    {
        if (nint.Zero == Handle)
            throw new InvalidOperationException("The stream is closed.");
        switch (_state)
        {
            case MidiStreamState.Paused:
            case MidiStreamState.Started:
                _DisposeTimer();
                _CheckOutResult(midiStreamStop(Handle));
                Interlocked.Exchange(ref _tempoSyncMessagesSentCount, 0);
                _state = MidiStreamState.Stopped;

                Interlocked.Exchange(ref _sendQueuePosition, 0);

                if (null != _sendQueue)
                {
                    Interlocked.Exchange(ref _sendQueue, null);
                }
                break;
        }
    }

    /// <summary>
    /// Pauses the stream
    /// </summary>
    public void Pause()
    {
        if (nint.Zero == Handle)
            throw new InvalidOperationException("The stream is closed.");
        switch (_state)
        {
            case MidiStreamState.Started:
                _CheckOutResult(midiStreamPause(Handle));
                _state = MidiStreamState.Paused;
                Interlocked.Exchange(ref _tempoSyncMessagesSentCount, 0);
                break;
        }
    }

    private static string _GetMidiOutErrorMessage(int errorCode)
    {
        var result = new StringBuilder(256);
        _ = midiOutGetErrorText(errorCode, result, result.Capacity);
        return result.ToString();
    }

    [System.Diagnostics.DebuggerNonUserCode()]
    private static void _CheckOutResult(int errorCode)
    {
        if (0 != errorCode)
            throw new Exception(_GetMidiOutErrorMessage(errorCode));
    }

    private void _SendBlock()
    {
        if (null == _sendQueue)
            return;
        if (nint.Zero == Handle)
            throw new InvalidOperationException("The stream is closed.");

        int blockSize = 0;
        nint headerPointer = Marshal.AllocHGlobal(MIDIHDR_SIZE + MAX_EVENTBLOCK_SIZE);
        try
        {
            nint eventPointer = new(headerPointer.ToInt64() + MIDIHDR_SIZE);
            var ofs = 0;
            var ptrOfs = 0;
            for (; _sendQueuePosition < _sendQueue.Count; Interlocked.Exchange(ref _sendQueuePosition, _sendQueuePosition + 1))
            {
                var @event = _sendQueue[_sendQueuePosition];
                if (@event.Message is null)
                    continue;
                if (0x00 != @event.Message.Status && 0xF0 != (@event.Message.Status & 0xF0))
                {
                    if (MAX_EVENTBLOCK_SIZE < blockSize + MIDIEVENT_SIZE)
                        break;
                    blockSize += MIDIEVENT_SIZE;
                    var se = default(MIDIEVENT);
                    se.dwDeltaTime = @event.Position + ofs;
                    se.dwStreamId = 0;
                    se.dwEvent = MidiUtility.PackMessage(@event.Message);
                    Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                    ptrOfs += MIDIEVENT_SIZE;
                    ofs = 0;
                }
                else if (0xFF == @event.Message.Status)
                {
                    var mm = (MidiMessageMeta)@event.Message;
                    if (0x51 == mm.Data1) // tempo
                    {
                        if (MAX_EVENTBLOCK_SIZE < blockSize + MIDIEVENT_SIZE)
                            break;
                        blockSize += MIDIEVENT_SIZE;
                        var se = default(MIDIEVENT);
                        se.dwDeltaTime = @event.Position + ofs;
                        se.dwStreamId = 0;
                        se.dwEvent = (mm.Data[0] << 16) | (mm.Data[1] << 8) | mm.Data[2] | (MEVT_TEMPO << 24);
                        Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                        ptrOfs += MIDIEVENT_SIZE;
                        ofs = 0;
                    }
                    else if (0x2f == mm.Data1) // end track
                    {
                        if (MAX_EVENTBLOCK_SIZE < blockSize + MIDIEVENT_SIZE)
                            break;
                        blockSize += MIDIEVENT_SIZE;

                        // add a NOP message to it just to pad our output in case we're looping
                        var se = default(MIDIEVENT);
                        se.dwDeltaTime = @event.Position + ofs;
                        se.dwStreamId = 0;
                        se.dwEvent = (MEVT_NOP << 24);
                        Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                        ptrOfs += MIDIEVENT_SIZE;
                        ofs = 0;
                    }
                    else
                        ofs = @event.Position;
                }
                else // sysex or sysex part
                {
                    byte[] data;
                    if (0 == @event.Message.Status)
                        data = ((MidiMessageSysexPart)@event.Message).Data;
                    else
                        data = MidiUtility.ToMessageBytes(@event.Message);

                    var dl = data.Length;
                    if (0 != (dl % 4))
                        dl += 4 - (dl % 4);
                    if (MAX_EVENTBLOCK_SIZE < blockSize + MIDIEVENT_SIZE + dl)
                        break;

                    blockSize += MIDIEVENT_SIZE + dl;

                    var se = default(MIDIEVENT);
                    se.dwDeltaTime = @event.Position + ofs;
                    se.dwStreamId = 0;
                    se.dwEvent = MEVT_F_LONG | data.Length;
                    Marshal.StructureToPtr(se, new nint(ptrOfs + eventPointer.ToInt64()), false);
                    ptrOfs += MIDIEVENT_SIZE;
                    Marshal.Copy(data, 0, new nint(ptrOfs + eventPointer.ToInt64()), data.Length);

                    ptrOfs += dl;
                    ofs = 0;
                }
            }
            var header = default(MIDIHDR);
            header.dwBufferLength = header.dwBytesRecorded = unchecked((uint)blockSize);
            header.lpData = eventPointer;
            Marshal.StructureToPtr(header, headerPointer, false);
            _CheckOutResult(midiOutPrepareHeader(Handle, headerPointer, MIDIHDR_SIZE));
            _CheckOutResult(midiStreamOut(Handle, headerPointer, MIDIHDR_SIZE));
            headerPointer = nint.Zero;
        }
        finally
        {
            if (nint.Zero != headerPointer)
                Marshal.FreeHGlobal(headerPointer);
        }
    }

    private void _RestartTimer(int ms)
    {
        if (0 >= ms)
            throw new ArgumentOutOfRangeException(nameof(ms));
        _DisposeTimer();
        var h = timeSetEvent(ms, 0, _timerCallback, nint.Zero, TIME_ONESHOT);
        if (nint.Zero == h)
            throw new Exception("Could not create multimedia timer");
        Interlocked.Exchange(ref _timerHandle, h);
    }

    private void _DisposeTimer()
    {
        //if(null!=_timerHandle)
        {
            _ = timeKillEvent(_timerHandle);
            Interlocked.Exchange(ref _timerHandle, nint.Zero);
        }
    }

    private void _TimerProc(nint handle, int msg, int user, nint param1, nint param2)
    {
        if (nint.Zero != Handle && _timerHandle == handle && 0 != _tempoSyncEnabled)
        {
            if (0 == _tempoSyncMessageCount || _tempoSyncMessagesSentCount < _tempoSyncMessageCount)
            {
                // quickly send a time sync message
                _ = midiOutShortMsg(Handle, 0xF8);
                Interlocked.Increment(ref _tempoSyncMessagesSentCount);
            }
            var tmp = Tempo;
            var spb = 60 / tmp;
            var ms = unchecked((int)(Math.Round((1000 * spb) / 24)));
            _RestartTimer(ms);
        }
    }

    private void _MidiOutProc(nint handle, int msg, int instance, nint param1, nint param2)
    {
        switch (msg)
        {
            case MOM_OPEN:
                OnOpened(EventArgs.Empty);
                break;
            case MOM_CLOSE:
                OnClosed(EventArgs.Empty);
                break;
            case MOM_DONE:

                if (nint.Zero != param1)
                {
                    _ = Marshal.PtrToStructure<MIDIHDR>(param1);
                    _CheckOutResult(midiOutUnprepareHeader(Handle, param1, Marshal.SizeOf(typeof(MIDIHDR))));
                    Marshal.FreeHGlobal(param1);
                    Interlocked.Exchange(ref _sendQueuePosition, 0);
                    Interlocked.Exchange(ref _sendQueue, null);
                }

                if (null == _sendQueue)
                    SendComplete?.Invoke(this, EventArgs.Empty);
                else
                {
                    _SendBlock();
                }
                break;
        }
    }
}
