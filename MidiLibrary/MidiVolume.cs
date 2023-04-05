namespace Midi;

/// <summary>
/// Represents the mono or stereo volume for a MIDI output device or string
/// </summary>
/// <param name="Left"> Indicates the left or mono volume </param>
/// <param name="Right"> Indicates the right volume </param>
public record struct MidiVolume(byte Left, byte Right)
{
    /// <summary>
    /// Creates a new instance of the structure
    /// </summary>
    /// <param name="mono">The mono volume</param>
    public MidiVolume(byte mono) : this(mono, 0)
    {
    }
}
