namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI note off message
/// </summary>
public class MidiMessageNoteOff : MidiMessageWord
{
    /// <summary>
    /// Creates a new MIDI note off message
    /// </summary>
    /// <param name="noteId">The MIDI note id (0-127)</param>
    /// <param name="velocity">The MIDI velocity (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    /// <remarks><paramref name="velocity"/> is not used</remarks>
    public MidiMessageNoteOff(byte noteId, byte velocity, byte channel) : base(unchecked((byte)(0x80 | channel)), noteId, velocity)
    {
    }

    /// <summary>
    /// Creates a new MIDI note off message
    /// </summary>
    /// <param name="note">The MIDI note</param>
    /// <param name="velocity">The MIDI velocity (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    /// <remarks><paramref name="velocity"/> is not used</remarks>
    public MidiMessageNoteOff(string note, byte velocity, byte channel) : this(MidiUtility.NoteToNoteId(note), velocity, channel)
    {
    }

    /// <summary>
    /// Indicates the MIDI note id to turn off
    /// </summary>
    public byte NoteId
    {
        get
        {
            return Data1;
        }
    }

    /// <summary>
    /// Indicates the note for the message
    /// </summary>
    public string Note
    {
        get
        {
            return MidiUtility.NoteIdToNote(NoteId, true);
        }
    }

    /// <summary>
    /// Indicates the velocity of the note to turn off
    /// </summary>
    /// <remarks>This value is not used</remarks>
    public byte Velocity
    {
        get
        {
            return Data2;
        }
    }

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "Note Off: " + MidiUtility.NoteIdToNote(NoteId) + ", Velocity: " + Velocity.ToString() + ", Channel: " + Channel.ToString();
    }

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl()
    {
        return new MidiMessageNoteOff(NoteId, Velocity, Channel);
    }
}
