using System.Text;

namespace OpenTibia.IO
{
    public class ByteArrayStreamWriter
    {
        public ByteArrayStreamWriter(ByteArrayStream stream) : this(stream, Encoding.GetEncoding("ISO-8859-1") )
        {

        }

        public ByteArrayStreamWriter(ByteArrayStream stream, Encoding encoding)
        {
            this.stream = stream;

            this.encoding = encoding;
        }

        private ByteArrayStream stream;

        public ByteArrayStream BaseStream
        {
            get
            {
                return stream;
            }
        }

        private Encoding encoding;

        public Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }

        private void InternalWriteInt32(int count, int value)
        {
            for (int i = 0; i < count; i++)
            {
                stream.WriteByte( (byte)( (value & (0xFF << (i * 8) ) ) >> (i * 8) ) );
            }
        }

        private void InternalWriteInt64(int count, long value)
        {
            for (int i = 0; i < count; i++)
            {
                stream.WriteByte( (byte)( (value & (0xFF << (i * 8) ) ) >> (i * 8) ) );
            }
        }

        public void Write(byte value)
        {
            stream.WriteByte(value);
        }
        
        public void Write(bool value)
        {
            if (value)
            {
                Write( (byte)0x01 );
            }
            else
            {
                Write( (byte)0x00 );
            }
        }

        public void Write(short value)
        {
            InternalWriteInt32(2, (int)value);
        }

        public void Write(ushort value)
        {
            InternalWriteInt32(2, (int)value);
        }

        public void Write(int value)
        {
            InternalWriteInt32(4, (int)value);
        }

        public void Write(uint value)
        {
            InternalWriteInt32(4, (int)value);
        }

        public void Write(long value)
        {
            InternalWriteInt64(8, (long)value);
        }

        public void Write(ulong value)
        {
            InternalWriteInt64(8, (long)value);
        }

        private static object locker = new object();

        private static byte[] buffer = new byte[65535];

        public void Write(string value)
        {
            int length;

            if (value == null || value == "")
            {
                length = 0;

                Write( (ushort)length );
            }
            else
            {
                length = value.Length;

                Write( (ushort)length );

                lock (locker)
                {
                    encoding.GetBytes(value, 0, length, buffer, 0);

                    Write(buffer, 0, length);
                }
            }
        }

        public void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            stream.Write(buffer, offset, length);
        }
    }
}