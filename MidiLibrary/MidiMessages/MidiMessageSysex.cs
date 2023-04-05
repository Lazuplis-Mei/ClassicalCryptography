using System.Text;
namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI system exclusive message with an arbitrary length payload
/// </summary>
public class MidiMessageSysex : MidiMessage
{
    /// <summary>
    /// Creates a MIDI message with the specified status, type and payload
    /// </summary>
    /// <param name="data">The payload of the MIDI message, as bytes</param>
    public MidiMessageSysex(byte[] data) : base(0xF0) => Data = data;

    /// <summary>
    /// Indicates the payload length for this MIDI message
    /// </summary>
    public override int PayloadLength => -1;

    /// <summary>
    /// Indicates the payload data, as bytes
    /// </summary>
    public byte[] Data { get; private set; }

    /// <summary>
    /// Returns a string representation of the message
    /// </summary>
    /// <returns>The string representation of the message</returns>
    public override string ToString() => $"Sysex: {Convert.ToHexString(Data)}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageSysex(Data);
}
