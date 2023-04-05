namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI stop message
/// </summary>
public sealed class MidiMessageRealTimeStop : MidiMessageRealTime
{
    /// <summary>
    /// Creates a new MIDI stop message
    /// </summary>
    public MidiMessageRealTimeStop() : base(0xFC)
    {
    }

    /// <summary>
    /// Indicates the payload of the message
    /// </summary>
    public override int PayloadLength => 0;

    /// <summary>
    /// Clones the message
    /// </summary>
    /// <returns>The new message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageRealTimeStop();
}
