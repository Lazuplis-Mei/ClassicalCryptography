namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI channel prefix meta message
/// </summary>
public class MidiMessageMetaChannelPrefix : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified channel
    /// </summary>
    /// <param name="channelPrefix">The channel (0-15)</param>
    public MidiMessageMetaChannelPrefix(byte channelPrefix) : base(0x20, new byte[] { unchecked((byte)(channelPrefix & 0x0F)) })
    {
    }

    internal MidiMessageMetaChannelPrefix(byte[] data) : base(0x20, data)
    {
    }

    /// <summary>
    /// Indicates the channel for the channel prefix
    /// </summary>
    public byte ChannelPrefix => Data[0];

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Channel Prefix: {ChannelPrefix}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaChannelPrefix(Data);
}
