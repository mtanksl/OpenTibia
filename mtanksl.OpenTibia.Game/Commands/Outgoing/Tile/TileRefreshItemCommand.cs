using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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

            foreach (var observer in Context.Server.Map.GetObservers(Tile.Position).OfType<Player>() )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Item, out clientIndex) )
                {
                    Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, clientIndex, Item) );

                    Context.AddEvent(observer, new TileRefreshItemEventArgs(Tile, Item, index) );
                }
            }

            Context.AddEvent(new TileRefreshItemEventArgs(Tile, Item, index) );

            return Promise.Completed;
        }
    }
}