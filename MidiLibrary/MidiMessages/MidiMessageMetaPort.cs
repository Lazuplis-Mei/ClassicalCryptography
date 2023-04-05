namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI port meta message
/// </summary>
public class MidiMessageMetaPort : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified port
    /// </summary>
    /// <param name="port">The port (0-127)</param>
    public MidiMessageMetaPort(byte port) : base(0x21, new byte[] { unchecked((byte)(port & 0x7F)) })
    {
    }

    internal MidiMessageMetaPort(byte[] data) : base(0x21, data)
    {
    }

    /// <summary>
    /// Indicates the port
    /// </summary>
    public byte Port => Data[0];

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Port: {Port}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaPort(Data);
}
