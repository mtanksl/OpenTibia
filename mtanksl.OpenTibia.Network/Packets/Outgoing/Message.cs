using OpenTibia.IO;
using OpenTibia.Security;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class Message : IEnumerable<IOutgoingPacket>
    {
        private List<IOutgoingPacket> packets = new List<IOutgoingPacket>(1);

               IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IOutgoingPacket> GetEnumerator()
        {
            return packets.GetEnumerator();
        }
        
        public Message Add(IOutgoingPacket packet)
        {
            packets.Add(packet);

            return this;
        }

        public Message Add(params IOutgoingPacket[] packet)
        {
            packets.AddRange(packet);

            return this;
        }

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
            ByteArrayMemoryStream stream = new ByteArrayMemoryStream();

            ByteArrayStreamWriter writer = new ByteArrayStreamWriter(stream);

            foreach (var packet in packets)
            {
                packet.Write(writer);
            }

            if (keys == null)
            {
                return Length(Hash(Length(stream.GetBytes() ) ) );
            }

            return Length(Hash(Encrypt(keys, Length(stream.GetBytes() ) ) ) );
        }
    }
}