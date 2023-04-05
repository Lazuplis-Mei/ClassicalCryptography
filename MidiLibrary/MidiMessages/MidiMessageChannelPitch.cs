namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI channel pitch/pitch wheel message
/// </summary>
public class MidiMessageChannelPitch : MidiMessageWord
{
    /// <summary>
    /// Creates a new MIDI channel pitch message
    /// </summary>
    /// <param name="pitch">The MIDI pressure (0-16383)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageChannelPitch(short pitch, byte channel) : base(unchecked((byte)(0xE0 | channel)), BitConverter.IsLittleEndian ? MidiUtility.Swap(pitch) : pitch)
    {
    }

    internal MidiMessageChannelPitch(byte pitch1, byte pitch2, byte channel) : base(unchecked((byte)(0xE0 | channel)), pitch1, pitch2)
    {
    }

    /// <summary>
    /// Indicates the pitch of the channel (pitch wheel position)
    /// </summary>
    public short Pitch
    {
        get
        {
            if (BitConverter.IsLittleEndian)
                return MidiUtility.Swap(Data);
            return Data;
        }
    }

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Channel Pitch: {Pitch}, Channel: {Channel}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageChannelPitch(Data1, Data2, Channel);
}
