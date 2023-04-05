namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI sequence/track name meta message
/// </summary>
public class MidiMessageMetaSequenceOrTrackName : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaSequenceOrTrackName(string text) : base(3, text ?? "")
    {
    }

    internal MidiMessageMetaSequenceOrTrackName(byte[] data) : base(3, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Sequence/Track Name: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaSequenceOrTrackName(Data);
}
