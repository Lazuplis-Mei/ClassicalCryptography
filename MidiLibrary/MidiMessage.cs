namespace Midi;

/// <summary>
/// Represents a MIDI message
/// </summary>
public partial class MidiMessage : ICloneable
{
    /// <summary>
    /// Creates a MIDI message with the specified status byte
    /// </summary>
    /// <param name="status">The MIDI status byte</param>
    public MidiMessage(byte status) => Status = status;

    /// <summary>
    /// Indicates the MIDI status byte
    /// </summary>
    public byte Status { get; private set; }

    /// <summary>
    /// Indicates the channel of the MIDI message. Only applies to MIDI channel messages, not MIDI system messages
    /// </summary>
    public byte Channel => unchecked((byte)(Status & 0xF));

    /// <summary>
    /// Indicates the length of the message payload
    /// </summary>
    public virtual int PayloadLength => 0;

    /// <summary>
    /// Creates a deep copy of the message
    /// </summary>
    /// <returns>A message that is equivelent to the specified message</returns>
    public MidiMessage Clone() => CloneImpl();

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected virtual MidiMessage CloneImpl() => new(Status);
}
