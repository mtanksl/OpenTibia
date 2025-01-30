using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ThingUpdateOutgoingPacket : IOutgoingPacket
    {
        private int option;

        /// <summary>
        /// Creature.
        /// </summary>
        public ThingUpdateOutgoingPacket(Position position, byte index, uint creatureId, Direction direction)
        {
            this.option = 1;


            this.Position = position;

            this.Index = index;

            this.CreatureId = creatureId;

            this.Direction = direction;
        }

        /// <summary>
        /// Item.
        /// </summary>
        public ThingUpdateOutgoingPacket(Position position, byte index, Item item)
        {
            this.option = 2;


            this.Position = position;

            this.Index = index;

            this.Item = item;
        }

        public Position Position { get; set; }

        public byte Index { get; set; }

        public uint CreatureId { get; set; }

        public Direction Direction { get; set; }

        public Item Item { get; set; }
              
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6B );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            writer.Write(Index);

            switch (option)
            {
                case 1:

                    writer.Write( (ushort)0x63 );

                    writer.Write(CreatureId);

                    writer.Write( (byte)Direction );

                    break;

                case 2:

                    writer.Write(Item);

                    break;
            }
        }
    }
}