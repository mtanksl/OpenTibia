using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class LoginCommand : Command
    {
        private Server server;

        public LoginCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            //Act

            server.CreatureCollection.AddCreature(Player);

            byte fromIndex = fromTile.AddContent(Player);

            AddEvent(new LoggedInEventArgs(server)
            {
                Player = Player,

                FromTile = fromTile,

                FromIndex = fromIndex
            } );
        }
    }
}