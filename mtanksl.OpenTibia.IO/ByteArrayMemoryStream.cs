using System.Collections.Generic;

namespace OpenTibia.IO
{
    public class ByteArrayMemoryStream : ByteArrayStream
    {
        private List<byte> bytes = new List<byte>();

        public override byte ReadByte()
        {
            byte value = bytes[position];

            Seek(Origin.Current, 1);

            return value;
        }

        public override void Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i + offset] = ReadByte();
            }
        }

        public override void WriteByte(byte value)
        {
            bytes.Add(value);

            Seek(Origin.Current, 1);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                WriteByte( buffer[i + offset] );
            }
        }

        public byte[] GetBytes()
        {
            return bytes.ToArray();
        }
    }
}