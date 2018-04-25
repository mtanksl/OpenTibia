using OpenTibia.IO;

namespace OpenTibia
{
    public class ThingAddOutgoingPacket : IOutgoingPacket
    {
        private int option;

        /// <summary>
        /// Known creature.
        /// </summary>
        public ThingAddOutgoingPacket(Position position, byte index, Creature creature)
        {
            this.option = 1;


            this.Position = position;

            this.Index = index;

            this.Creature = creature;
        }

        /// <summary>
        /// Unknown creature.
        /// </summary>
        public ThingAddOutgoingPacket(Position position, byte index, uint removeId, Creature creature)
        {
            this.option = 2;


            this.Position = position;

            this.Index = index;

            this.RemoveId = removeId;

            this.Creature = creature;
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
                
        public Item Item { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x6A );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            writer.Write(Index);

            switch (option)
            {
                case 1:

                    writer.Write(Creature);

                    break;

                case 2:

                    writer.Write(RemoveId, Creature);

                    break;

                case 3:

                    writer.Write(Item);

                    break;
            }
        }
    }
}