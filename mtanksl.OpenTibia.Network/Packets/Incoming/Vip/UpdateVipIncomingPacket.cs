using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class UpdateVipIncomingPacket : IIncomingPacket
    {
        public uint CreatureId { get; set; }

        public string Description { get; set; }

        public uint IconId { get; set; }

        public bool NotifyLogin { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            CreatureId = reader.ReadUInt();

            Description = reader.ReadString();

            IconId = reader.ReadUInt();

            NotifyLogin = reader.ReadBool();
        }
    }
}