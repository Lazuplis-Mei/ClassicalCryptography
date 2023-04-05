namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI active sensing message
/// </summary>
public sealed class MidiMessageRealTimeActiveSensing : MidiMessageRealTime
{
    /// <summary>
    /// Creates a new MIDI active sensing message
    /// </summary>
    public MidiMessageRealTimeActiveSensing() : base(0xFE)
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
    protected override MidiMessage CloneImpl() => new MidiMessageRealTimeActiveSensing();
}
