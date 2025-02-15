using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class TileAddItemCommand : Command
    {
        public TileAddItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            int index = Tile.AddContent(Item);

            var canSeeTo = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Tile.Position) )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Item, out clientIndex) )
                {
                    canSeeTo.Add(observer, clientIndex);
                }
            }

            foreach (var pair in canSeeTo)
            {
                Context.AddPacket(pair.Key, new ThingAddOutgoingPacket(Tile.Position, pair.Value, Item) );
            }
                        
            Context.AddEvent(new TileAddItemEventArgs(Tile, Item, index) );

            return Promise.Completed;
        }
    }
}