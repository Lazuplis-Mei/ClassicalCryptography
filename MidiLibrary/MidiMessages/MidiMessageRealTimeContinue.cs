namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI continue message
/// </summary>
public sealed class MidiMessageRealTimeContinue : MidiMessageRealTime
{
    /// <summary>
    /// Creates a new MIDI continue message
    /// </summary>
    public MidiMessageRealTimeContinue() : base(0xFB)
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
    protected override MidiMessage CloneImpl() => new MidiMessageRealTimeContinue();
}
