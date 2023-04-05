namespace Midi;

/// <summary>
/// Represents a single MIDI event
/// </summary>
public partial class MidiEvent : ICloneable
{
    /// <summary>
    /// Creates an event at the specified position with the specified MIDI message
    /// </summary>
    /// <param name="position">The position in MIDI ticks</param>
    /// <param name="message">The MIDI message for this event</param>
    public MidiEvent(int position, MidiMessage? message)
    {
        Position = position;
        Message = message;
    }

    /// <summary>
    /// Indicates the position in MIDI ticks
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Indicates the MIDI message associated with this event
    /// </summary>
    public MidiMessage? Message { get; private set; }

    /// <summary>
    /// Creates a deep copy of the MIDI event
    /// </summary>
    /// <returns>A new, equivelent MIDI event</returns>
    public MidiEvent Clone() => new(Position, Message?.Clone());

    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Returns a string representation of the event
    /// </summary>
    /// <returns>A string representation of the event</returns>
    public override string ToString()
    {
        if (Message is null)
            return Position.ToString();
        return $"{Position}::{Message}";
    }
}
