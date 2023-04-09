using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileReplaceItemCommand : Command
    {
        public TileReplaceItemCommand(Tile tile, Item fromItem, Item toItem)
        {
            Tile = tile;

            FromItem = fromItem;

            ToItem = toItem;
        }

        public Tile Tile { get; set; }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }
        
        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                byte index = Tile.GetIndex(FromItem);

                Tile.ReplaceContent(index, ToItem);

                if (index < Constants.ObjectsPerPoint)
                {
                    foreach (var observer in context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer.Tile.Position.CanSee(Tile.Position) )
                        {
                            context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Tile.Position, index, ToItem) );
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}