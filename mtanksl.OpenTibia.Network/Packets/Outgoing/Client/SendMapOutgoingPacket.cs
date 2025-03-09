using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public abstract class SendMapOutgoingPacket : IOutgoingPacket
    {
        private const byte Separator = 0xFF;

        private IMapGetTile map;

        private IClient client;

        public SendMapOutgoingPacket(IMapGetTile map, IClient client)
        {
            this.map = map;

            this.client = client;
        }

        public void GetMapDescription(IByteArrayStreamWriter writer, IHasFeatureFlag features, int x, int y, int z, int width, int height, int floor, int floors)
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

                empty = GetFloorDescription(writer, features, x, y, currentFloor, z - currentFloor, width, height, empty);
            }

            if (empty != -1)
            {
                writer.Write( (byte)empty );

                writer.Write(Separator);
            }
        }

        private int GetFloorDescription(IByteArrayStreamWriter writer, IHasFeatureFlag features, int x, int y, int z, int offset, int width, int height, int empty)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tile tile = map.GetTile(new Position(x + i + offset, y + j + offset, z) );

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

                        GetTileDescription(writer, features, tile);
                    }
                }
            }

            return empty;
        }

        private void GetTileDescription(IByteArrayStreamWriter writer, IHasFeatureFlag features, Tile tile)
        {
            byte index = 0;

            foreach (var content in tile.GetContents() )
            {
                if (index >= 10) // Constants.ObjectsPerPoint
                {
                    break;
                }

                switch (content)
                {
                    case Item item:

                        writer.Write(features, item);

                        break;

                    case Creature creature:

                        if (creature != client.Player && creature.Invisible)
                        {
                            continue;
                        }

                        uint removeId;

                        if (client.Battles.IsKnownCreature(creature.Id, out removeId) )
                        {
                            writer.Write(features, creature, client.GetClientSkullIcon(creature), client.GetClientPartyIcon(creature) );
                        }
                        else
                        {
                            writer.Write(features, removeId, creature, client.GetClientSkullIcon(creature), client.GetClientPartyIcon(creature), client.GetClientWarIcon(creature) );
                        }

                        break;
                }

                index++;
            }
        }

        public abstract void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features);
    }
}