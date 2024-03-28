using System;
using System.Text;

namespace OpenTibia.IO
{
    public class ByteArrayStreamReader
    {
        public ByteArrayStreamReader(ByteArrayStream stream) : this(stream, Encoding.GetEncoding("ISO-8859-1") )
        {
            
        }

        public ByteArrayStreamReader(ByteArrayStream stream, Encoding encoding)
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

        public byte ReadByte()
        {
            return stream.ReadByte();
        }
        
        public bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadBytes(1), 0);
        }
        
        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadBytes(2), 0);
        }
        
        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadBytes(2), 0);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(ReadBytes(4), 0);          
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(ReadBytes(8), 0);
        }

        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(ReadBytes(8), 0);
        }

        public string ReadString()
        {
            return encoding.GetString( ReadBytes( ReadUShort() ) );
        }

        public byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length]; 

            stream.Read(buffer, 0, buffer.Length);
            
            return buffer;
        }
    }
}