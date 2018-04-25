using System;

namespace OpenTibia.IO
{
    public class ByteArrayArrayStream : ByteArrayStream
    {
        private byte[] bytes;

        public ByteArrayArrayStream(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public override byte ReadByte()
        {
            byte value = bytes[position];

            Seek(Origin.Current, 1);

            return value;
        }

        public override void Read(byte[] buffer, int offset, int count)
        {
            Buffer.BlockCopy(bytes, position, buffer, offset, count);

            Seek(Origin.Current, count);
        }

        public override void WriteByte(byte value)
        {
            bytes[position] = value;

            Seek(Origin.Current, 1);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Buffer.BlockCopy(buffer, offset, bytes, position, count);

            Seek(Origin.Current, count);
        }
    }
}