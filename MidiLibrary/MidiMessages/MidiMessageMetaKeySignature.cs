namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI time signature meta message
/// </summary>
public class MidiMessageMetaKeySignature : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified tempo
    /// </summary>
    /// <param name="keySignature">The time signature</param>
    public MidiMessageMetaKeySignature(MidiKeySignature keySignature) : base(0x59, new byte[] {
                    unchecked((byte)(0<keySignature.FlatsCount?-keySignature.FlatsCount:keySignature.SharpsCount)),
                    unchecked((byte)(keySignature.IsMinor?1:0))})
    {
    }

    internal MidiMessageMetaKeySignature(byte[] data) : base(0x59, data)
    {
    }

    /// <summary>
    /// Indicates the key signature
    /// </summary>
    public MidiKeySignature KeySignature => new(unchecked((sbyte)Data[0]), 0 != Data[1]);

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Key Signature: {KeySignature}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaKeySignature(Data);
}
