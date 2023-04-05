namespace Midi;

public sealed partial class MidiContext
{
    /// <summary>
    /// Represents the status of a MIDI channel
    /// </summary>
    public sealed partial class Channel
    {
        /// <summary>
        /// MIDI channel
        /// </summary>
        public Channel()
        {
            Controls = new byte[128];
            for (int i = 0; i < 128; ++i) Controls[i] = 0xFF;
            Notes = new byte[128];
            for (int i = 0; i < 128; ++i) Notes[i] = 0xFF;
            KeyPressure = new byte[128];
            for (int i = 0; i < 128; ++i) KeyPressure[i] = 0xFF;
            ChannelPressure = 0xFF;
            PitchWheel = -1;
            Program = 0xFF;
        }

        /// <summary>
        /// Indicates the current MIDI CC values for the channel
        /// </summary>
        public byte[] Controls { get; internal set; }

        /// <summary>
        /// Indicates the current MIDI note states for the channel
        /// </summary>
        public byte[] Notes { get; internal set; }

        /// <summary>
        /// Indicates the current MIDI aftertouch for the current key
        /// </summary>
        public byte[] KeyPressure { get; internal set; }

        /// <summary>
        /// Indicates the current MIDI aftertouch for the current channel
        /// </summary>
        public byte ChannelPressure { get; internal set; }

        /// <summary>
        /// Indicates the current position of the MIDI pitch wheel
        /// </summary>
        public short PitchWheel { get; internal set; }

        /// <summary>
        /// Indicates the current MIDI patch for the channel
        /// </summary>
        public byte Program { get; internal set; }
    }
}
