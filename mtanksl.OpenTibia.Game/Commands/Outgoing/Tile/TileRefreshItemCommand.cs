using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileRefreshItemCommand : Command
    {
        public TileRefreshItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            byte index = Tile.GetIndex(Item);

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Item, out clientIndex) )
                {
                    Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, clientIndex, Item) );
                }
            }

            return Promise.Completed;
        }
    }
}