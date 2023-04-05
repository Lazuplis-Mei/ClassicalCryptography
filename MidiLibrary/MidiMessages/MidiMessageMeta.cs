using System.Text;

namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI meta-event message with an arbitrary length payload
/// </summary>
public class MidiMessageMeta : MidiMessageByte
{
    /// <summary>
    /// Creates a MIDI message with the specified status, type and payload
    /// </summary>
    /// <param name="type">The type of the MIDI message</param>
    /// <param name="data">The payload of the MIDI message, as bytes</param>
    public MidiMessageMeta(byte type, byte[] data) : base(0xFF, type) => Data = data;

    /// <summary>
    /// Creates a MIDI message with the specified status, type and payload
    /// </summary>
    /// <param name="type">The type of the MIDI message</param>
    /// <param name="text">The payload of the MIDI message, as ASCII text</param>
    public MidiMessageMeta(byte type, string text) : base(0xFF, type)
    {
        Data = Encoding.ASCII.GetBytes(text);
    }

    /// <summary>
    /// Indicates the type of the meta-message
    /// </summary>
    public byte Type => Data1;

    /// <summary>
    /// Indicates the payload length for this MIDI message
    /// </summary>
    public override int PayloadLength => -1;

    /// <summary>
    /// Indicates the payload data, as bytes
    /// </summary>
    public byte[] Data { get; private set; }

    /// <summary>
    /// Indicates the payload data, as ASCII text
    /// </summary>
    public string Text => Encoding.ASCII.GetString(Data);

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Meta: {Type:x2}, Length: {Data.Length}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMeta(Type, Data);
}
