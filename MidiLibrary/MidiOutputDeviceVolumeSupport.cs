namespace Midi;

/// <summary>
/// Represents the kind of volume settings are available for the device
/// </summary>
public enum MidiOutputDeviceVolumeSupport
{
    /// <summary>
    /// Volume controls are not available
    /// </summary>
    None = 0,

    /// <summary>
    /// Only mono or single volume controls are supported
    /// </summary>
    Mono = 1,

    /// <summary>
    /// Stereo volume controls are supported
    /// </summary>
    Stereo = 2
}
