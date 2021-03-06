﻿using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            byte index = Tile.AddContent(Item);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Item) );
                }
            }

            //Event

            if (server.Events.TileAddItem != null)
            {
                server.Events.TileAddItem(this, new TileAddItemEventArgs(Item, Tile, index, server, context) );
            }

            base.Execute(server, context);
        }
    }
}