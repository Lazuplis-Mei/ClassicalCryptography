namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI marker meta message
/// </summary>
public class MidiMessageMetaMarker : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaMarker(string text) : base(6, text ?? "")
    {
    }

    internal MidiMessageMetaMarker(byte[] data) : base(6, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Marker: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaMarker(Data);
}
