namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI message a payload word (2 bytes)
/// </summary>
public class MidiMessageWord : MidiMessageByte
{
    /// <summary>
    /// Creates a MIDI message with the specified status and payload
    /// </summary>
    /// <param name="status">The MIDI status byte</param>
    /// <param name="data1">The first data byte</param>
    /// <param name="data2">The second data byte</param>
    public MidiMessageWord(byte status, byte data1, byte data2) : base(status, data1) { Data2 = data2; }

    /// <summary>
    /// Creates a MIDI message with the specified status and payload
    /// </summary>
    /// <param name="status">The MIDI status byte</param>
    /// <param name="data">The data word</param>
    public MidiMessageWord(byte status, short data) : base(
        status,
        unchecked((byte)(data & 0x7F)))
    {
        Data2 = unchecked((byte)((data / 256) & 0x7F));
    }

    /// <summary>
    /// Indicates the payload length for this MIDI message
    /// </summary>
    public override int PayloadLength => 2;

    /// <summary>
    /// Indicates the second data byte
    /// </summary>
    public byte Data2 { get; private set; }

    /// <summary>
    /// Indicates the data word
    /// </summary>
    public short Data => unchecked((short)(Data1 + (Data2 * 256)));

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageWord(Status, Data1, Data2);
}
