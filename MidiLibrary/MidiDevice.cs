﻿using System.Runtime.InteropServices;

namespace Midi;

/// <summary>
/// Represents a base class for MIDI input and output devices
/// </summary>
public abstract class MidiDevice : IDisposable
{
    #region Win32

    [DllImport("winmm.dll")]
    private static extern int midiOutGetNumDevs();

    [DllImport("winmm.dll")]
    private static extern int midiInGetNumDevs();

    #endregion Win32

    /// <summary>
    /// Closes the device if it's open
    /// </summary>
    ~MidiDevice()
    {
        Close();
    }

    /// <summary>
    /// Indicates the available MIDI input devices
    /// </summary>
    public static IList<MidiInputDevice> Inputs
    {
        get
        {
            var count = midiInGetNumDevs();
            var result = new List<MidiInputDevice>(count);
            for (var i = 0; i < count; i++)
                result.Add(new MidiInputDevice(i));
            return result;
        }
    }

    /// <summary>
    /// Indicates the available MIDI output devices
    /// </summary>
    public static IList<MidiOutputDevice> Outputs
    {
        get
        {
            var count = midiOutGetNumDevs();
            var result = new List<MidiOutputDevice>(count);
            for (var i = 0; i < count; i++)
                result.Add(new MidiOutputDevice(i));
            return result;
        }
    }

    /// <summary>
    /// Indicates the available MIDI streaming devices
    /// </summary>
    public static IList<MidiStream> Streams
    {
        get
        {
            var count = midiOutGetNumDevs();
            var result = new List<MidiStream>(count);
            for (var i = 0; i < count; i++)
                result.Add(new MidiStream(i));
            return result;
        }
    }

    /// <summary>
    /// Indicates the name of the MIDI device
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Indicates the version of the device
    /// </summary>
    public abstract Version Version { get; }

    /// <summary>
    /// Indicates the manufacturer code for the device
    /// </summary>
    public abstract short ManufacturerId { get; }

    /// <summary>
    /// Indicates the product code for the device
    /// </summary>
    public abstract short ProductId { get; }

    /// <summary>
    /// Indicates the index of the MIDI device
    /// </summary>
    public abstract int Index { get; }

    /// <summary>
    /// Indicates whether or not this device is open
    /// </summary>
    public abstract bool IsOpen { get; }

    /// <summary>
    /// Opens the MIDI device
    /// </summary>
    public abstract void Open();

    /// <summary>
    /// Closes the MIDI device
    /// </summary>
    public abstract void Close();

    void IDisposable.Dispose()
    {
        Close();
        GC.SuppressFinalize(this);
    }
}
