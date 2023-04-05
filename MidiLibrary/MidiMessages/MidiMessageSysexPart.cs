namespace Midi.MidiMessages;

// internal class used by the framework
internal sealed class MidiMessageSysexPart : MidiMessage
{
    internal MidiMessageSysexPart(byte[] data) : base(0) => Data = data;

    public byte[] Data { get; set; }

    public override int PayloadLength => -1;

    /// <summary>
    /// When overridden in a derived class, implements Clone()
    /// </summary>
    /// <returns>The cloned MIDI message</returns>
    protected override MidiMessage CloneImpl() => new MidiMessageSysexPart(Data);
}
