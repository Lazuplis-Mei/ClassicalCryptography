namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI note on message
/// </summary>
public class MidiMessageNoteOn : MidiMessageWord
{
    /// <summary>
    /// Creates a new MIDI note on message
    /// </summary>
    /// <param name="noteId">The MIDI note id (0-127)</param>
    /// <param name="velocity">The MIDI velocity (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageNoteOn(byte noteId, byte velocity, byte channel) : base(unchecked((byte)(0x90 | channel)), noteId, velocity)
    {
    }

    /// <summary>
    /// Creates a new MIDI note on message
    /// </summary>
    /// <param name="note">The MIDI note</param>
    /// <param name="velocity">The MIDI velocity (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageNoteOn(string note, byte velocity, byte channel) : this(MidiUtility.NoteToNoteId(note), velocity, channel)
    {
    }

    /// <summary>
    /// Indicates the MIDI note id to play
    /// </summary>
    public byte NoteId => Data1;

    /// <summary>
    /// Indicates the note for the message
    /// </summary>
    public string Note => MidiUtility.NoteIdToNote(NoteId, true);

    /// <summary>
    /// Indicates the velocity of the note to play
    /// </summary>
    public byte Velocity => Data2;

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Note On: {Note}, Velocity: {Velocity}, Channel: {Channel}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageNoteOn(NoteId, Velocity, Channel);
}
