using Midi.MidiMessages;

namespace Midi;

/// <summary>
/// Represents a MIDI program name meta message
/// </summary>
public class MidiMessageMetaProgramName : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaProgramName(string text) : base(8, text ?? "")
    {
    }

    internal MidiMessageMetaProgramName(byte[] data) : base(8, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Program Name: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaProgramName(Data);
}
