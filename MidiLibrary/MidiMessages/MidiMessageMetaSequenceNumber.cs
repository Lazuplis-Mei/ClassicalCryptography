namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI sequence number meta message
/// </summary>
public class MidiMessageMetaSequenceNumber : MidiMessageMeta
{
    /// <summary>
    /// Creates a new message with the specified sequence number
    /// </summary>
    /// <param name="sequenceNumber">The sequence number</param>
    public MidiMessageMetaSequenceNumber(short sequenceNumber) : base(0, new byte[] { unchecked((byte)(sequenceNumber & 0x7F)), unchecked((byte)((sequenceNumber / 256) & 0x7F)) })
    {
    }

    /// <summary>
    /// Creates a new message with the default sequence number
    /// </summary>
    public MidiMessageMetaSequenceNumber() : base(0, Array.Empty<byte>())
    {
    }

    internal MidiMessageMetaSequenceNumber(byte[] data) : base(0, data)
    {
    }

    /// <summary>
    /// Indicates the sequence number, or -1 if there was none specified
    /// </summary>
    public short SequenceNumber
    {
        get
        {
            if (Data.Length == 0)
                return -1;
            return unchecked((short)(Data[0] + Data[1] * 256));
        }
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Sequence Number: {(0 == Data.Length ? "<default>" : SequenceNumber.ToString())}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaSequenceNumber(Data);
}
