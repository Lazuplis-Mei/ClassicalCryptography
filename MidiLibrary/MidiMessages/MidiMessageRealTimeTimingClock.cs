namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI start message
/// </summary>
public sealed class MidiMessageRealTimeTimingClock : MidiMessageRealTime
{
    /// <summary>
    /// Creates a new MIDI timing clock message
    /// </summary>
    public MidiMessageRealTimeTimingClock() : base(0xF8)
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
    protected override MidiMessage CloneImpl() => new MidiMessageRealTimeTimingClock();
}
