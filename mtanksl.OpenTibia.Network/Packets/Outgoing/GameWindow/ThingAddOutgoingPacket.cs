using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ThingAddOutgoingPacket : IOutgoingPacket
    {
        private int option;

        /// <summary>
        /// Known creature.
        /// </summary>
        public ThingAddOutgoingPacket(Position position, byte index, Creature creature, SkullIcon skullIcon, PartyIcon partyIcon)
        {
            this.option = 1;


            this.Position = position;

            this.Index = index;

            this.Creature = creature;

            this.SkullIcon = skullIcon;

            this.PartyIcon = partyIcon;
        }

        /// <summary>
        /// Unknown creature.
        /// </summary>
        public ThingAddOutgoingPacket(Position position, byte index, uint removeId, Creature creature, SkullIcon skullIcon, PartyIcon partyIcon, WarIcon warIcon)
        {
            this.option = 2;


            this.Position = position;

            this.Index = index;

            this.RemoveId = removeId;

            this.Creature = creature;

            this.SkullIcon = skullIcon;

            this.PartyIcon = partyIcon;

            this.WarIcon = warIcon;
        }

        /// <summary>
        /// Item.
        /// </summary>
        public ThingAddOutgoingPacket(Position position, byte index, Item item)
        {
            this.option = 3;


            this.Position = position;

            this.Index = index;

            this.Item = item;
        }

        public Position Position { get; set; }

        public byte Index { get; set; }

        public uint RemoveId { get; set; }

        public Creature Creature { get; set; }

        public SkullIcon SkullIcon { get; set; }

        public PartyIcon PartyIcon { get; set; }

        public WarIcon WarIcon { get; set; }

        public Item Item { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x6A );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            if (features.HasFeatureFlag(FeatureFlag.TileIndex) )
            {
                writer.Write(Index);
            }

            switch (option)
            {
                case 1:

                    writer.Write(features, Creature, SkullIcon, PartyIcon);

                    break;

                case 2:

                    writer.Write(features, RemoveId, Creature, SkullIcon, PartyIcon, WarIcon);

                    break;

                case 3:

                    writer.Write(Item);

                    break;
            }
        }
    }
}