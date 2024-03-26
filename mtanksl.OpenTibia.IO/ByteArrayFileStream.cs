using System;
using System.IO;

namespace OpenTibia.IO
{
    public class ByteArrayFileStream : ByteArrayBufferedStream
    {
        public ByteArrayFileStream(string path) : base( new FileStream(path, FileMode.Open) )
        {

        }

        public override byte ReadByte()
        {
            return GetByte();
        }

        public override void Read(byte[] buffer, int offset, int count)
        {
            GetBytes(buffer, offset, count);
        }

        /// <exception cref="NotSupportedException"></exception>
       
        public override void WriteByte(byte value)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="NotSupportedException"></exception>

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}