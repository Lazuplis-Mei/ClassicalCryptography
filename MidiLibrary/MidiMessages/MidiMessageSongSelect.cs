namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI song select message
/// </summary>
public sealed class MidiMessageSongSelect : MidiMessageByte
{
    /// <summary>
    /// Creates a new MIDI song select message
    /// </summary>
    /// <param name="songId">Indicates the new song id</param>
    public MidiMessageSongSelect(byte songId) : base(0xF3, songId)
    {
    }

    /// <summary>
    /// Indicates the song id
    /// </summary>
    public byte SongId => Data1;

    /// <summary>
    /// Clones the message
    /// </summary>
    /// <returns>The new message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageSongSelect(SongId);
}
