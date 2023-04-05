namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI copyright meta message
/// </summary>
public class MidiMessageMetaCopyright : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaCopyright(string text) : base(2, text ?? "")
    {
    }

    internal MidiMessageMetaCopyright(byte[] data) : base(2, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Copyright: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaCopyright(Data);
}
