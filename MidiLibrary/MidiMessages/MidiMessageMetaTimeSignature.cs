using Midi.MidiMessages;

namespace Midi;

/// <summary>
/// Represents a MIDI time signature meta message
/// </summary>
public class MidiMessageMetaTimeSignature : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified tempo
    /// </summary>
    /// <param name="timeSignature">The time signature</param>
    public MidiMessageMetaTimeSignature(MidiTimeSignature timeSignature) : base(0x58, new byte[] {
                    timeSignature.Numerator,
                    unchecked((byte)(Math.Log(timeSignature.Denominator)/Math.Log(2))),
                    timeSignature.MidiTicksPerMetronomeTick,
                    timeSignature.ThirtySecondNotesPerQuarterNote })
    {
    }

    internal MidiMessageMetaTimeSignature(byte[] data) : base(0x58, data)
    {
    }

    /// <summary>
    /// Indicates the time signature
    /// </summary>
    public MidiTimeSignature TimeSignature
    {
        get
        {
            var num = Data[0];
            var den = Data[1];
            var met = Data[2];
            var q32 = Data[3];
            return new MidiTimeSignature(num, (short)Math.Pow(2, den), met, q32);
        }
    }

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Time Signature: {TimeSignature}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaTimeSignature(Data);
}
