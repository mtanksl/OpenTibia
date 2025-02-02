using OpenTibia.Game.Common;
using OpenTibia.IO;
using OpenTibia.Security;
using System;
using System.Net.Sockets;

namespace OpenTibia.Common
{
    public abstract class TibiaConnection : RateLimitingConnection
    {
        public TibiaConnection(IServer server, Socket socket) : base(server, socket)
        {

        }

        protected override byte[] Envelope(byte[] bytes)
        {
            if (Keys == null)
            {
                return Length(Hash(Length(bytes) ) );
            }

            return Length(Hash(Encrypt(Keys, Length(bytes) ) ) );
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
    }
}