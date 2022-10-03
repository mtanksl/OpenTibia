﻿using OpenTibia.Common.Objects;
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
        
        public override void Execute(Context context)
        {
            byte index = Tile.GetIndex(Item);

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, index, Item) );
                }
            }

            OnComplete(context);
        }
    }
}