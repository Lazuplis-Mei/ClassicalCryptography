namespace Midi;

/// <summary>
/// Indicates the state of the MIDI stream
/// </summary>
public enum MidiStreamState
{
    /// <summary>
    /// The stream is closed
    /// </summary>
    Closed = -1,

    /// <summary>
    /// The stream is paused
    /// </summary>
    Paused = 0,

    /// <summary>
    /// The stream is stopped
    /// </summary>
    Stopped = 1,

    /// <summary>
    /// The stream is playing
    /// </summary>
    Started = 2
}
