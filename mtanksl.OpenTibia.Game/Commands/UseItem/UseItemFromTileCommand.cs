using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromTileCommand : Command
    {
        private Server server;

        public UseItemFromTileCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }
        
        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            Item fromItem = (Item)fromTile.GetContent(FromIndex);

            //Act



            //Notify

            
        }
    }
}