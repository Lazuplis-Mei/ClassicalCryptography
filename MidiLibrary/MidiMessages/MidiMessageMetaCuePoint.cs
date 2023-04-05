namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI cue point meta message
/// </summary>
public class MidiMessageMetaCuePoint : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaCuePoint(string text) : base(7, text ?? "")
    {
    }

    internal MidiMessageMetaCuePoint(byte[] data) : base(7, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Cue Point: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaCuePoint(Data);
}
