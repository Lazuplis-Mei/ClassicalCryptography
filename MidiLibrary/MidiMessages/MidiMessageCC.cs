namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI continuous controller message
/// </summary>
internal partial class MidiMessageCC : MidiMessageWord
{
    /// <summary>
    /// Creates a new MIDI continuous controller message
    /// </summary>
    /// <param name="controlId">The MIDI controller id (0-127)</param>
    /// <param name="value">The MIDI value (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageCC(byte controlId, byte value, byte channel) : base(unchecked((byte)(0xB0 | channel)), controlId, value)
    {
    }

    /// <summary>
    /// Indicates the assocated MIDI controller id
    /// </summary>
    public byte ControlId => Data1;

    /// <summary>
    /// Indicates the value of the controller
    /// </summary>
    public byte Value => Data2;

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"CC: {ControlId}, Value: {Value}, Channel: {Channel}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageCC(ControlId, Value, Channel);
}
