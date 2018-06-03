using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class SelectedOutfitCommand : Command
    {
        private Server server;

        public SelectedOutfitCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Outfit Outfit { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            //Act

            Player.Outfit = Outfit;

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(fromTile.Position) )
                {
                    context.Response.Write(observer.Client.Connection, new SetOutfit(Player.Id, Outfit) );
                }
            }
        }
    }
}