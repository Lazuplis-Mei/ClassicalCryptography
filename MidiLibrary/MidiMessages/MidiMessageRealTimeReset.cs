namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI reset message
/// </summary>
/// <remarks>This message shares the same status code with MIDI a meta-message. The meta-messages come from files but this comes over the wire whereas meta-messages do not.</remarks>
public sealed class MidiMessageRealTimeReset : MidiMessageRealTime
{
    /// <summary>
    /// Creates a new MIDI reset message
    /// </summary>
    public MidiMessageRealTimeReset() : base(0xFF)
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
    protected override MidiMessage CloneImpl() => new MidiMessageRealTimeReset();
}
