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

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                byte index = Tile.AddContent(Item);

                if (index < Constants.ObjectsPerPoint)
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer.Tile.Position.CanSee(Tile.Position) )
                        {
                            Context.AddPacket(observer.Client.Connection, new ThingAddOutgoingPacket(Tile.Position, index, Item) );
                        }
                    }
                }

                resolve();
            } );
        }
    }
}