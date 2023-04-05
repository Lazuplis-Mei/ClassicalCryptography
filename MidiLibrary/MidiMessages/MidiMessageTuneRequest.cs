namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI tune request message
/// </summary>
public sealed class MidiMessageTuneRequest : MidiMessage
{
    /// <summary>
    /// Creates a new MIDI tune request message
    /// </summary>
    public MidiMessageTuneRequest() : base(0xF6)
    {
    }

    /// <summary>
    /// Clones the message
    /// </summary>
    /// <returns>The new message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageTuneRequest();
}
