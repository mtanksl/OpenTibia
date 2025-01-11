namespace OpenTibia.IO
{
    public class ByteArrayMemoryFileTreeStream : ByteArrayMemoryStream
    {
        public const byte Escape = 0xFD;

        public const byte Start = 0xFE;

        public const byte End = 0xFF;

        public override byte ReadByte()
        {
            byte value = base.ReadByte();

            if (value == Escape)
            {
                value = base.ReadByte();
            }

            return value;
        }

        public override void WriteByte(byte value)
        {
            if (value == Escape || value == Start || value == End)
            {
                base.WriteByte(Escape);
            }

            base.WriteByte(value);
        }

        public void StartChild()
        {
            base.WriteByte(Start);
        }

        public void EndChild()
        {
            base.WriteByte(End);
        }
    }
}