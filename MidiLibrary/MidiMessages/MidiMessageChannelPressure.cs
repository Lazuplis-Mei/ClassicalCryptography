namespace Midi.MidiMessages;

/// <summary>
/// Represents a MIDI key pressure/aftertouch message
/// </summary>
public class MidiMessageChannelPressure : MidiMessageByte
{
    /// <summary>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </summary>
    /// <param name="pressure">The MIDI pressure (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public MidiMessageChannelPressure(byte pressure, byte channel) : base(unchecked((byte)(0xD0 | channel)), pressure)
    {
    }

    /// <summary>
    /// Indicates the pressure of the channel (aftertouch)
    /// </summary>
    /// <remarks>Indicates the single greatest pressure/aftertouch off all pressed notes</remarks>
    public byte Pressure => Data1;

    /// <summary>
    /// Gets a string representation of this message
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Channel Pressure: {Pressure}, Channel: {Channel}";

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageChannelPressure(Pressure, Channel);
}
