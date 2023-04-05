namespace Midi;

/// <summary>
/// Indicates the state of the MIDI input device
/// </summary>
public enum MidiInputDeviceState
{
    /// <summary>
    /// The device is closed
    /// </summary>
    Closed = -1,

    /// <summary>
    /// The device has been started
    /// </summary>
    Started = 0,

    /// <summary>
    /// The device is stopped
    /// </summary>
    Stopped = 1
}
