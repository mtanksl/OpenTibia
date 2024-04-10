using OpenTibia.IO;
using OpenTibia.Security;
using System;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Message
    {
        private ByteArrayMemoryStream stream;

        private ByteArrayStreamWriter writer;

        public Message()
        {
            stream = new ByteArrayMemoryStream();

            writer = new ByteArrayStreamWriter(stream);
        }

        public Message Add(IOutgoingPacket packet)
        {
            packet.Write(writer);

            return this;
        }

        public Message Add(IOutgoingPacket[] packets)
        {
            foreach (var packet in packets)
            {
                packet.Write(writer);
            }

            return this;
        }

        //TODO: Reduce allocation

        private byte[] Length(byte[] bytes)
        {
            byte[] length = BitConverter.GetBytes( (ushort)bytes.Length );

            return length.Combine(bytes);
        }

        private byte[] Hash(byte[] bytes)
        {
            byte[] hash = BitConverter.GetBytes( Adler32.Generate(bytes) );

            return hash.Combine(bytes);
        }
        
        private byte[] Encrypt(uint[] keys, byte[] bytes)
        {
            int padding = bytes.Length % 8;

            if (padding > 0)
            {
                bytes = bytes.Combine( new byte[8 - padding] );
            }

            return Xtea.Encrypt(bytes, 32, keys);
        }

        public byte[] GetBytes(uint[] keys)
        {
            if (keys == null)
            {
                return Length(Hash(Length(stream.GetBytes() ) ) );
            }

            return Length(Hash(Encrypt(keys, Length(stream.GetBytes() ) ) ) );
        }
    }
}