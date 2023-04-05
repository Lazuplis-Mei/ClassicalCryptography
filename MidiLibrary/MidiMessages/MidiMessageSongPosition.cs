namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI song position message
/// </summary>
public sealed class MidiMessageSongPosition : MidiMessageWord
{
    /// <summary>
    /// Creates a new MIDI song position message
    /// </summary>
    /// <param name="position">Indicates the new song position in beats since the start</param>
    public MidiMessageSongPosition(short position) : base(0xF2, position)
    {
    }

    /// <summary>
    /// Creates a new MIDI song position message
    /// </summary>
    /// <param name="position1">The high part of the position</param>
    /// <param name="position2">The low part of the position</param>
    public MidiMessageSongPosition(byte position1, byte position2) : base(0xF2, position1, position2)
    {
    }

    /// <summary>
    /// Indicates the new song position in beats since the start
    /// </summary>
    public short Position => Data;

    /// <summary>
    /// Clones the message
    /// </summary>
    /// <returns>The new message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageSongPosition(Position);
}
