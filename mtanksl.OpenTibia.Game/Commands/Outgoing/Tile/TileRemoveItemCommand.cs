using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

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
            return Promise.Run( (resolve, reject) =>
            {
                byte index = Tile.GetIndex(Item);

                Tile.RemoveContent(index);

                if (index < Constants.ObjectsPerPoint || Tile.Count >= Constants.ObjectsPerPoint)
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer.Tile.Position.CanSee(Tile.Position) )
                        {
                            if (index < Constants.ObjectsPerPoint)
                            {
                                Context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, index) );
                            }

                            if (Tile.Count >= Constants.ObjectsPerPoint)
                            {
                                Context.AddPacket(observer.Client.Connection, new SendTileOutgoingPacket(Context.Server.Map, observer.Client, Tile.Position) );
                            }
                        }
                    }
                }

                resolve();
            } );
        }
    }
}