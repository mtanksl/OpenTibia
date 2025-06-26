using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class VipOutgoingPacket : IOutgoingPacket
    {
        public VipOutgoingPacket(uint id, string name, string description, uint iconId, bool notifyLogin, bool online)
        {
            this.Id = id;

            this.Name = name;

            this.Description = description;

            this.IconId = iconId;

            this.NotifyLogin = notifyLogin;

            this.Online = online;
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public uint IconId { get; set; }

        public bool NotifyLogin { get; set; }

        public bool Online { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xD2 );

            writer.Write(Id);

            writer.Write(Name);

            if (features.HasFeatureFlag(FeatureFlag.AdditionalVipInfo) )
            {
                writer.Write(Description);

                writer.Write(IconId);

                writer.Write(NotifyLogin);
            }

            writer.Write(Online);
        }
    }
}