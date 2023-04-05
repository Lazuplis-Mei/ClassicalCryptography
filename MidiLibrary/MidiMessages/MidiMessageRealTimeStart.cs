namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI start message
/// </summary>
public sealed class MidiMessageRealTimeStart : MidiMessageRealTime
{
    /// <summary>
    /// Creates a new MIDI start message
    /// </summary>
    public MidiMessageRealTimeStart() : base(0xFA)
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
    protected override MidiMessage CloneImpl() => new MidiMessageRealTimeStart();
}
