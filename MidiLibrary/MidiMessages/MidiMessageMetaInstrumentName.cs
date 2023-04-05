namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI instrument name meta message
/// </summary>
public class MidiMessageMetaInstrumentName : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaInstrumentName(string text) : base(4, text ?? "")
    {
    }

    internal MidiMessageMetaInstrumentName(byte[] data) : base(4, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Instrument Name: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaInstrumentName(Data);
}
