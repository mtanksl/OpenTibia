using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.IO
{
    public static class ByteArrayStreamReaderExtensions
    {
        public static Outfit ReadOutfit(this IByteArrayStreamReader reader)
        {
            ushort id = reader.ReadUShort();

            if (id == 0)
            {
                return new Outfit( reader.ReadUShort() );
            }

            return new Outfit( id, reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), (Addon)reader.ReadByte() );
        }

        public static Light ReadLight(this IByteArrayStreamReader reader)
        {
            return new Light( reader.ReadByte(), reader.ReadByte() );
        }

        public static string ReadCsd(this IByteArrayStreamReader reader)
        {
            byte[] bytes = reader.ReadBytes(128);
            
            int index = Array.FindIndex(bytes, b => b == 0);

            if (index == -1)
	        {
                 return reader.Encoding.GetString(bytes);
	        }

            return reader.Encoding.GetString(bytes, index, bytes.Length - index);
        }
    }
}