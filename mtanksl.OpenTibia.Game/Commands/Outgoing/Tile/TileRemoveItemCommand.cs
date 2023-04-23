using OpenTibia.Common.Objects;
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
            var updates = new Queue< (Player Player, byte ClientIndex) >();

            foreach (var observer in Context.Server.GameObjects.GetPlayers() )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(Item, out clientIndex) )
                {
                    updates.Enqueue( (observer, clientIndex) );
                }
            }

            byte index = Tile.GetIndex(Item);

            Tile.RemoveContent(index);

            while (updates.Count > 0)
            {
                var update = updates.Dequeue();

                if (Tile.Count >= Constants.ObjectsPerPoint)
                {
                    Context.AddPacket(update.Player.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, update.Player.Client, Tile.Position) );
                }
                else
                {
                    Context.AddPacket(update.Player.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, update.ClientIndex) );
                }
            }

            Context.AddEvent(new TileRemoveItemEventArgs(Tile, Item, index) );

            return Promise.Completed;
        }
    }
}