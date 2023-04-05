namespace Midi;

/// <summary>
/// Represents the kind of MIDI output device
/// </summary>
public enum MidiOutputDeviceKind
{
    /// <summary>
    /// Unknown MIDI device.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// MIDI Port
    /// </summary>
    MidiPort = 1,

    /// <summary>
    /// Synthesizer
    /// </summary>
    Synthesizer = 2,

    /// <summary>
    /// Square wave synthesizer
    /// </summary>
    SquareWaveSynthesizer = 3,

    /// <summary>
    /// FM synthesizer
    /// </summary>
    FMSynthesizer = 4,

    /// <summary>
    /// MIDI mapper
    /// </summary>
    MidiMapper = 5,

    /// <summary>
    /// Wavetable synthesizer
    /// </summary>
    WavetableSynthesizer = 6,

    /// <summary>
    /// Software synthesizer
    /// </summary>
    SoftwareSynthesizer = 7
}
