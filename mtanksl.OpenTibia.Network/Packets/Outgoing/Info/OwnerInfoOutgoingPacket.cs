using OpenTibia.Common.Objects;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class OwnerInfoOutgoingPacket : IOutgoingPacket
    {
        public OwnerInfoOutgoingPacket(string ownerName, string ownerEmail)
        {
            OwnerName = ownerName;

            OwnerEmail = ownerEmail;
        }

        public string OwnerName { get; set; }

        public string OwnerEmail { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x11);

            writer.Write(OwnerName);

            writer.Write(OwnerEmail);
        }
    }
}