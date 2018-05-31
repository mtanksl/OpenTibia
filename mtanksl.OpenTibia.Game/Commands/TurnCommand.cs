using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class TurnCommand : Command
    {
        private Server server;

        public TurnCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Direction ToDirection { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            Direction fromDirection = Player.Direction;

            //Act

            Player.Direction = ToDirection;

            AddEvent(new TurnedEventArgs(server)
            {
                Player = Player,

                FromTile = fromTile,

                FromIndex = fromIndex,

                FromDirection = fromDirection,

                ToDirection = ToDirection
            } );
        }
    }
}