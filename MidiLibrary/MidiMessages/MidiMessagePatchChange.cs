namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI key pressure/aftertouch message
/// </summary>
public class MidiMessagePatchChange : MidiMessageByte
{
    /// <summary>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </summary>
    /// <param name="patchId">The MIDI patch Id (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessagePatchChange(byte patchId, byte channel) : base(unchecked((byte)(0xC0 | channel)), patchId)
    {
    }

    /// <summary>
    /// Indicates the assocated MIDI patch id
    /// </summary>
    public byte PatchId => Data1;

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Patch Change: {PatchId}, Channel: {Channel}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessagePatchChange(PatchId, Channel);
}
