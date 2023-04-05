namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI device port name meta message
/// </summary>
public class MidiMessageMetaDevicePortName : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified text
    /// </summary>
    /// <param name="text">The text</param>
    public MidiMessageMetaDevicePortName(string text) : base(9, text ?? "")
    {
    }

    internal MidiMessageMetaDevicePortName(byte[] data) : base(9, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Device Port Name: {Text}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaDevicePortName(Data);
}
