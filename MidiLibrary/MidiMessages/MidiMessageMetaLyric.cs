namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI lyric meta message
/// </summary>
public class MidiMessageMetaLyric : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaLyric(string text) : base(5, text ?? "")
    {
    }

    internal MidiMessageMetaLyric(byte[] data) : base(5, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Lyric: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaLyric(Data);
}
