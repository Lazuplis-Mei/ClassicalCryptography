namespace Midi;

/// <summary>
/// Represents the supported channels for the MIDI output device
/// </summary>
[Flags]
public enum MidiChannels : short
{
    /// <summary>
    /// Channel 0
    /// </summary>
    Channel0 = 0x0001,
    /// <summary>
    /// Channel 1
    /// </summary>
    Channel1 = 0x0002,
    /// <summary>
    /// Channel 2
    /// </summary>
    Channel2 = 0x0004,
    /// <summary>
    /// Channel 3
    /// </summary>
    Channel3 = 0x0008,
    /// <summary>
    /// Channel 4
    /// </summary>
    Channel4 = 0x0010,
    /// <summary>
    /// Channel 5
    /// </summary>
    Channel5 = 0x0020,
    /// <summary>
    /// Channel 6
    /// </summary>
    Channel6 = 0x0040,
    /// <summary>
    /// Channel 7
    /// </summary>
    Channel7 = 0x0080,
    /// <summary>
    /// Channel 8
    /// </summary>
    Channel8 = 0x0100,
    /// <summary>
    /// Channel 9
    /// </summary>
    Channel9 = 0x0200,
    /// <summary>
    /// Channel 10
    /// </summary>
    Channel10 = 0x0400,
    /// <summary>
    /// Channel 11
    /// </summary>
    Channel11 = 0x0800,
    /// <summary>
    /// Channel 12
    /// </summary>
    Channel12 = 0x1000,
    /// <summary>
    /// Channel 13
    /// </summary>
    Channel13 = 0x2000,
    /// <summary>
    /// Channel 14
    /// </summary>
    Channel14 = 0x4000,
    /// <summary>
    /// Channel 15
    /// </summary>
    Channel15 = unchecked((short)0x8000)
}
