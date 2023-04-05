using System.Runtime.InteropServices;
using System.Text;

namespace Midi;

/// <summary>
/// Represents a MIDI output device
/// </summary>
public class MidiOutputDevice : MidiDevice
{
    #region Win32

    private const int MHDR_DONE = 1;
    private const int MHDR_PREPARED = 2;
    private const uint MIDICAPS_VOLUME = 1;
    // supports volume control
    private const uint MIDICAPS_LRVOLUME = 2;
    // separate left-right volume control
    private const uint MIDICAPS_CACHE = 4;
    private const uint MIDICAPS_STREAM = 8;
    // driver supports midiStreamOut directly
    private const int CALLBACK_FUNCTION = 196608;
    private const int MOM_OPEN = 0x3C7;
    private const int MOM_CLOSE = 0x3C8;
    private delegate void MidiOutProc(nint handle, int msg, int instance, nint param1, nint param2);

    [DllImport("winmm.dll")]
    private static extern int midiOutGetDevCaps(int deviceIndex, ref MIDIOUTCAPS caps, int uSize);
    [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
    private static extern int midiOutGetErrorText(int errCode, StringBuilder message, int sizeOfMessage);
    [DllImport("winmm.dll")]
    private static extern int midiOutShortMsg(nint handle, int message);
    [DllImport("winmm.dll")]
    private static extern int midiOutLongMsg(nint hMidiOut, ref MIDIHDR lpMidiOutHdr, int uSize);
    [DllImport("winmm.dll")]
    private static extern int midiOutPrepareHeader(nint hMidiOut, ref MIDIHDR lpMidiOutHdr, int uSize);
    [DllImport("winmm.dll")]
    private static extern int midiOutUnprepareHeader(nint hMidiOut, ref MIDIHDR lpMidiOutHdr, int uSize);
    [DllImport("winmm.dll")]
    private static extern int midiOutOpen(ref nint handle, int deviceID, MidiOutProc proc, int instance, int flags);
    [DllImport("winmm.dll")]
    private static extern int midiOutClose(nint handle);
    [DllImport("winmm.dll")]
    private static extern int midiOutGetVolume(nint handle, out int volume);
    [DllImport("winmm.dll")]
    private static extern int midiOutSetVolume(nint handle, int volume);
    [DllImport("winmm.dll")]
    private static extern int midiOutReset(nint handle);
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
    private struct MIDIOUTCAPS
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;     //MMVERSION

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;

        public ushort wTechnology;
        public short wVoices;
        public short wNotes;
        public ushort wChannelMask;
        public uint dwSupport;
    }

    #endregion Win32

    private readonly MidiOutProc _outCallback;
    private readonly int _index;
    private readonly MIDIOUTCAPS _caps;
    private nint _handle;

    internal MidiOutputDevice(int index)
    {
        _index = index;
        _CheckOutResult(midiOutGetDevCaps(index, ref _caps, Marshal.SizeOf(typeof(MIDIOUTCAPS))));
        _handle = nint.Zero;
        _outCallback = new MidiOutProc(_MidiOutProc);
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
    /// Indicates the name of the MIDI output device
    /// </summary>
    public override string Name => _caps.szPname;

    /// <summary>
    /// Indicates the kind of MIDI output device
    /// </summary>
    public MidiOutputDeviceKind Kind => (MidiOutputDeviceKind)_caps.wTechnology;

    /// <summary>
    /// Indicates whether or not the device supports hardware accelerated streaming
    /// </summary>
    public virtual bool SupportsHardwareStreaming => MIDICAPS_STREAM == (_caps.dwSupport & MIDICAPS_STREAM);

    /// <summary>
    /// Indicates whether or not the device supports patch caching
    /// </summary>
    public bool SupportsPatchCaching => MIDICAPS_CACHE == (_caps.dwSupport & MIDICAPS_CACHE);

    /// <summary>
    /// Indicates the channels which the MIDI device responds to
    /// </summary>
    /// <remarks>These are flags</remarks>
    public MidiChannels Channels => (MidiChannels)_caps.wChannelMask;

    /// <summary>
    /// Indicates the number of voices the device supports or 0 if it can't be determined
    /// </summary>
    public short VoiceCount => _caps.wVoices;

    /// <summary>
    /// Indicates the number of simultaneous notes the device supports or 0 if it can't be determined
    /// </summary>
    public short NoteCount => _caps.wNotes;

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

    /// <summary>
    /// Indicates what kind of volume control is supported, if any
    /// </summary>
    public virtual MidiOutputDeviceVolumeSupport VolumeSupport
    {
        get
        {
            if (MIDICAPS_VOLUME == (_caps.dwSupport & MIDICAPS_VOLUME))
            {
                if (MIDICAPS_LRVOLUME == (_caps.dwSupport & MIDICAPS_LRVOLUME))
                    return MidiOutputDeviceVolumeSupport.Stereo;
                return MidiOutputDeviceVolumeSupport.Mono;
            }
            return MidiOutputDeviceVolumeSupport.None;
        }
    }

    /// <summary>
    /// Indicates the volume of the device
    /// </summary>
    public MidiVolume Volume
    {
        get
        {
            if (nint.Zero == _handle)
                throw new InvalidOperationException("The device is closed.");
            _CheckOutResult(midiOutGetVolume(_handle, out int vol));
            return new MidiVolume(unchecked((byte)(vol & 0xFF)), unchecked((byte)(vol >> 8)));
        }
        set
        {
            if (nint.Zero == _handle)
                throw new InvalidOperationException("The device is closed.");
            _CheckOutResult(midiOutSetVolume(_handle, value.Right << 8 | value.Left));
        }
    }

    /// <summary>
    /// Indicates the device index of the MIDI output device
    /// </summary>
    public override int Index => _index;

    /// <summary>
    /// Indicates whether or not this device is open
    /// </summary>
    public override bool IsOpen => nint.Zero != _handle;

    /// <summary>
    /// Retrieves the MIDI stream associated with this output device
    /// </summary>
    public MidiStream Stream => new(Index);

    /// <summary>
    /// Indicates the handle of the device
    /// </summary>
    protected nint Handle
    {
        get => _handle;
        set => Interlocked.Exchange(ref _handle, value);
    }

    /// <summary>
    /// Opens the MIDI output device
    /// </summary>
    public override void Open()
    {
        Close();
        _CheckOutResult(midiOutOpen(ref _handle, _index, _outCallback, 0, CALLBACK_FUNCTION));
        if (nint.Zero == _handle)
            throw new InvalidOperationException("Unable to open MIDI output device");
    }

    /// <summary>
    /// Closes the MIDI output device
    /// </summary>
    public override void Close()
    {
        if (nint.Zero != _handle)
        {
            _ = midiOutClose(_handle);
            _handle = nint.Zero;
        }
    }

    /// <summary>
    /// Sends a message out immediately
    /// </summary>
    /// <param name="message">The message to send</param>
    public void Send(MidiMessage message)
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The device is closed.");
        if (null == message)
            throw new ArgumentNullException(nameof(message));
        if (0xF0 == message.Status) // sysex
        {
            var data = MidiUtility.ToMessageBytes(message);
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
                    _SendRaw(data, i, len);
                }
            }
            else
                _SendRaw(data, 0, data.Length);
        }
        else
        {
            _CheckOutResult(midiOutShortMsg(_handle, MidiUtility.PackMessage(message)));
        }
    }

    /// <summary>
    /// Resets the MIDI output.
    /// </summary>
    /// <remarks>Terminates any sysex messages and sends note offs to all channels, as well as turning off the sustain controller for each channel</remarks>
    public void Reset()
    {
        if (nint.Zero == _handle)
            throw new InvalidOperationException("The stream is closed.");
        _CheckOutResult(midiOutReset(_handle));
    }

    /// <summary>
    /// Invokes the opened event
    /// </summary>
    protected void OnOpened(EventArgs args)
    {
        Opened?.Invoke(this, args);
    }

    /// <summary>
    /// Invokes the closed event
    /// </summary>
    protected void OnClosed(EventArgs args)
    {
        Closed?.Invoke(this, args);
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

    private void _SendRaw(byte[] data, int startIndex, int length)
    {
        var hdrSize = Marshal.SizeOf(typeof(MIDIHDR));
        var hdr = new MIDIHDR();
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        try
        {
            hdr.lpData = new nint(handle.AddrOfPinnedObject().ToInt64() + startIndex);
            hdr.dwBufferLength = hdr.dwBytesRecorded = (uint)(length);
            hdr.dwFlags = 0;
            _CheckOutResult(midiOutPrepareHeader(_handle, ref hdr, hdrSize));
            while ((hdr.dwFlags & MHDR_PREPARED) != MHDR_PREPARED)
            {
                Thread.Sleep(1);
            }
            _CheckOutResult(midiOutLongMsg(_handle, ref hdr, hdrSize));
            while ((hdr.dwFlags & MHDR_DONE) != MHDR_DONE)
            {
                Thread.Sleep(1);
            }
            _CheckOutResult(midiOutUnprepareHeader(_handle, ref hdr, hdrSize));
        }
        finally
        {
            handle.Free();
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
        }
    }
}
