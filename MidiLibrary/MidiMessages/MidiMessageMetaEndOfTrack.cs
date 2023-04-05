namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI end of track meta message
/// </summary>
public class MidiMessageMetaEndOfTrack : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance
    /// </summary>
    public MidiMessageMetaEndOfTrack() : base(0x2F, Array.Empty<byte>()) { }

    internal MidiMessageMetaEndOfTrack(byte[] data) : base(0x2F, data)
    {
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => "<End of Track>";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaEndOfTrack(Data);
}
