namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI text meta message
/// </summary>
public class MidiMessageMetaText : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaText(string text) : base(1, text ?? "")
    {
    }

    internal MidiMessageMetaText(byte[] data) : base(1, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Text: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaText(Data);
}
