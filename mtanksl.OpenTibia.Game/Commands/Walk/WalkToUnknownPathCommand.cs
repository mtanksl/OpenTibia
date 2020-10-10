using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class WalkToUnknownPathCommand : Command
    {
        public WalkToUnknownPathCommand(Player player, Tile tile)
        {
            Player = player;

            Tile = tile;
        }

        public Player Player { get; set; }

        public Tile Tile { get; set; }

        public override void Execute(Context context)
        {
            MoveDirection[] moveDirections = context.Server.Pathfinding.GetMoveDirections(Player.Tile.Position, Tile.Position);

            if (moveDirections.Length == 0)
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );
            }
            else
            {
                Command command = new WalkToKnownPathCommand(Player, moveDirections);

                command.Completed += (s, e) =>
                {
                    base.OnCompleted(e.Context);
                };

                command.Execute(context);
            }
        }
    }
}