using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToTileCommand : Command
    {
        private Server server;

        public MoveItemFromTileToTileCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public Position ToPosition { get; set; }
        
        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            Item fromItem = (Item)fromTile.GetContent(FromIndex);

            Tile toTile = server.Map.GetTile(ToPosition);

            //Act

            fromTile.RemoveContent(fromItem);

            byte toIndex = toTile.AddContent(fromItem);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(toTile.Position) )
                {
                     context.Response.Write(observer.Client.Connection, new ThingRemove(fromTile.Position, FromIndex) )
                        
                        .Write(observer.Client.Connection, new ThingAdd(toTile.Position, toIndex, fromItem) );
                }
            }
        }
    }
}