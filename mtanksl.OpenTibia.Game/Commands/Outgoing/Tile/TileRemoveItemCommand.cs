using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveItemCommand : CommandResult<byte>
    {
        public TileRemoveItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override PromiseResult<byte> Execute(Context context)
        {
            return PromiseResult<byte>.Run(resolve =>
            {
                byte index = Tile.GetIndex(Item);

                Tile.RemoveContent(index);

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(Tile.Position) )
                    {
                        if (index < Constants.ObjectsPerPoint)
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );
                        }

                        if (Tile.Count >= Constants.ObjectsPerPoint)
                        {
                            context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(context.Server.Map, observer.Client, Tile.Position) );
                        }
                    }
                }

                resolve(context, index);
            } );
        }
    }
}