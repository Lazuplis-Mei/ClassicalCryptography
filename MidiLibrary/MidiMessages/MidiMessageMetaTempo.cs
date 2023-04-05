namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI tempo meta message
/// </summary>
public class MidiMessageMetaTempo : MidiMessageMeta
{
    /// <summary>
    /// Creates a new instance with the specified tempo
    /// </summary>
    /// <param name="tempo">The tempo</param>
    public MidiMessageMetaTempo(double tempo) : this(MidiUtility.TempoToMicroTempo(tempo))
    {
    }

    /// <summary>
    /// Creates a new instance with the specified microtempo
    /// </summary>
    /// <param name="microTempo">The microtempo</param>
    public MidiMessageMetaTempo(int microTempo) : base(0x51, BitConverter.IsLittleEndian ?
                            new byte[] { unchecked((byte)(microTempo >> 16)), unchecked((byte)((microTempo >> 8) & 0xFF)), unchecked((byte)(microTempo & 0xFF)) } :
                            new byte[] { unchecked((byte)(microTempo & 0xFF)), unchecked((byte)((microTempo >> 8) & 0xFF)), unchecked((byte)(microTempo >> 16)) })
    { }

    internal MidiMessageMetaTempo(byte[] data) : base(0x51, data)
    {
    }

    /// <summary>
    /// Indicates the microtempo of the MIDI message
    /// </summary>
    public int MicroTempo
    {
        get
        {
            return BitConverter.IsLittleEndian ?
                (Data[0] << 16) | (Data[1] << 8) | Data[2] :
                (Data[2] << 16) | (Data[1] << 8) | Data[0];
        }
    }

    /// <summary>
    /// Indicates the tempo of the MIDI message
    /// </summary>
    public double Tempo => MidiUtility.MicroTempoToTempo(MicroTempo);

    /// <summary>
    /// Retrieves a string representation of the message
    /// </summary>
    /// <returns>A string representing the message</returns>
    public override string ToString() => $"Tempo: {Tempo}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageMetaTempo(Data);
}
