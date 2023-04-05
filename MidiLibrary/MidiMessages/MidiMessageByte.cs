namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI message with a single payload byte
/// </summary>
public class MidiMessageByte : MidiMessage
{
    /// <summary>
    /// Creates a MIDI message with the specified status and payload
    /// </summary>
    /// <param name="status">The MIDI status byte</param>
    /// <param name="data1">The data byte</param>
    public MidiMessageByte(byte status, byte data1) : base(status) { Data1 = data1; }

    /// <summary>
    /// Indicates the data byte for the MIDI message
    /// </summary>
    public byte Data1 { get; private set; }

    /// <summary>
    /// Indicates the payload length for this MIDI message
    /// </summary>
    public override int PayloadLength => 1;

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageByte(Status, Data1);
}
