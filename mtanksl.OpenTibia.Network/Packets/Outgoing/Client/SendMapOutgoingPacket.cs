using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Network.Packets.Outgoing
{
    public abstract class SendMapOutgoingPacket : IOutgoingPacket
    {
        private const byte Separator = 0xFF;

        private IMap map;

        private IClient client;

        public SendMapOutgoingPacket(IMap map, IClient client)
        {
            this.map = map;

            this.client = client;
        }

        public void Write(ByteArrayStreamWriter writer, int x, int y, int z, int width, int height, int floor, int floors)
        {
            int step = -1;

            if (floors > 0)
            {
                step = 1;
            }

            int empty = -1;

            for (int currentFloor = floor; currentFloor != (floor + floors + step); currentFloor += step)
            {
                if (currentFloor < 0 || currentFloor > 15)
                {
                    break;
                }

                int offset = z - currentFloor;
                
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Tile tile = map.GetTile(new Position(x + i + offset, y + j + offset, currentFloor) );

                        if (tile == null)
                        {
                            if (++empty == 255)
                            {
                                writer.Write( (byte)empty );

                                writer.Write(Separator);

                                empty = -1;
                            }
                        }
                        else
                        {
                            if (empty != -1)
                            {
                                writer.Write( (byte)empty );

                                writer.Write(Separator);
                            }

                            empty = 0;
                            
                            foreach (var content in tile.GetContents().Take(10) )
                            {
                                switch (content)
                                {
                                    case Item item:

                                        writer.Write(item);

                                        break;

                                    case Creature creature:

                                        uint removeId;

                                        if (client.CreatureCollection.IsKnownCreature(creature.Id, out removeId) )
                                        {
                                            writer.Write(creature);
                                        }
                                        else
                                        {
                                            writer.Write(removeId, creature);
                                        }

                                        break;
                                }
                            }
                        }
                    }
                }
            }

            if (empty != -1)
            {
                writer.Write( (byte)empty );

                writer.Write(Separator);
            }
        }

        public abstract void Write(ByteArrayStreamWriter writer);
    }
}