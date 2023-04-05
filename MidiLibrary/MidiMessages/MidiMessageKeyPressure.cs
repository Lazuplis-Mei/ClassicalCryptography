namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI key pressure/aftertouch message
/// </summary>
public class MidiMessageKeyPressure : MidiMessageWord
{
    /// <summary>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </summary>
    /// <param name="noteId">The MIDI note id (0-127)</param>
    /// <param name="pressure">The MIDI pressure (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageKeyPressure(byte noteId, byte pressure, byte channel) : base(unchecked((byte)(0xA0 | channel)), noteId, pressure)
    {
    }

    /// <summary>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </summary>
    /// <param name="note">The MIDI note</param>
    /// <param name="pressure">The MIDI pressure (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageKeyPressure(string note, byte pressure, byte channel) : this(MidiUtility.NoteToNoteId(note), pressure, channel)
    {
    }

    /// <summary>
    /// Indicates the assocated MIDI note id
    /// </summary>
    public byte NoteId => Data1;

    /// <summary>
    /// Indicates the note for the message
    /// </summary>
    public string Note => MidiUtility.NoteIdToNote(NoteId, true);

    /// <summary>
    /// Indicates the pressure of the note (aftertouch)
    /// </summary>
    public byte Pressure => Data2;

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Key Pressure: {Note}, Pressure: {Pressure}, Channel: {Channel}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageKeyPressure(NoteId, Pressure, Channel);
}
