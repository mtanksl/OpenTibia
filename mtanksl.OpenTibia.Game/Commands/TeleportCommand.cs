using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class TeleportCommand : Command
    {
        private Server server;

        public TeleportCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Tile FromTile { get; set; }

        public Tile ToTile { get; set; }

        public override void Execute(Context context)
        {
            //Act

            byte fromIndex = FromTile.RemoveContent(Player);

            byte toIndex = ToTile.AddContent(Player);

            AddEvent(new WalkedEventArgs(server)
            {
                Player = Player,

                FromTile = FromTile,

                FromIndex = fromIndex,

                ToTile = ToTile,

                ToIndex = toIndex
            } );
        }
    }
}