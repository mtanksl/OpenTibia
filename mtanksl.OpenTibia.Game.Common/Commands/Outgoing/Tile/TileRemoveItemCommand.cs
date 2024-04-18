using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveItemCommand : Command
    {
        public TileRemoveItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            var canSeeFrom = new Dictionary<Player, byte>();

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Tile.Position) )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Item, out clientIndex) )
                {
                    canSeeFrom.Add(observer, clientIndex);
                }
            }

            int index = Tile.GetIndex(Item);

            Tile.RemoveContent(index);

            foreach (var pair in canSeeFrom)
            {
                if (Tile.Count >= Constants.ObjectsPerPoint)
                {
                    Context.AddPacket(pair.Key, new SendTileOutgoingPacket(Context.Server.Map, pair.Key.Client, Tile.Position) );
                }
                else
                {
                    Context.AddPacket(pair.Key, new ThingRemoveOutgoingPacket(Tile.Position, pair.Value) );
                }
            }

            Context.AddEvent(new TileRemoveItemEventArgs(Tile, Item, index) );

            return Promise.Completed;
        }
    }
}