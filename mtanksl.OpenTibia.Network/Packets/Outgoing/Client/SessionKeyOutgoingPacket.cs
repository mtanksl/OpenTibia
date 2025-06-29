using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SessionKeyOutgoingPacket : IOutgoingPacket
    {
        public SessionKeyOutgoingPacket(string account, string password, string authenticatorCode)
        {
            this.Account = account;

            this.Password = password;

            this.AuthenticatorCode = authenticatorCode;
        }

        public string Account { get; set; }

        public string Password { get; set; }

        public string AuthenticatorCode { get; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x28 );

            writer.Write(Account + "\n" + Password + "\n" + AuthenticatorCode);
        }
    }
}