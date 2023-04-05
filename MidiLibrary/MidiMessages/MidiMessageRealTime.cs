namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI real-time message
/// </summary>
public abstract class MidiMessageRealTime : MidiMessage
{
    /// <summary>
    /// Creates a MIDI real-time message
    /// </summary>
    /// <param name="status"></param>
    protected MidiMessageRealTime(byte status) : base(status)
    {
    }
}
