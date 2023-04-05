namespace Midi;

/// <summary>
/// Represents an SMPTE timestamp
/// </summary>
/// <param name="Time"> The time </param>
/// <param name="Frames"> The frames </param>
/// <param name="FramesPerSecond"> The frames per second </param>
public readonly record struct MidiSmpteTime(TimeSpan Time, byte Frames, byte FramesPerSecond);
