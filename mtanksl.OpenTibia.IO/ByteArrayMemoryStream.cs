using System;
using System.Collections.Generic;

namespace OpenTibia.IO
{
    public class ByteArrayMemoryStream : ByteArrayStream
    {
        private List<byte> bytes = new List<byte>();

        public override byte ReadByte()
        {
            throw new NotSupportedException();
        }

        public override void Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void WriteByte(byte value)
        {
            bytes.Add(value);

            Seek(Origin.Current, 1);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < count; i++)
            {
                bytes.Add( buffer[i] );
            }

            Seek(Origin.Current, count);
        }

        public byte[] GetBytes()
        {
            return bytes.ToArray();
        }
    }
}