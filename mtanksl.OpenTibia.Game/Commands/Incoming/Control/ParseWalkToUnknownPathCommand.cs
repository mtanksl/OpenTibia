using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseWalkToUnknownPathCommand : Command
    {
        public ParseWalkToUnknownPathCommand(Player player, Tile tile)
        {
            Player = player;

            Tile = tile;
        }

        public Player Player { get; set; }

        public Tile Tile { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                MoveDirection[] moveDirections = Context.Server.Pathfinding.GetMoveDirections(Player.Tile.Position, Tile.Position);

                if (moveDirections.Length == 0)
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNoWay) );
                }
                else
                {
                    Context.AddCommand(new ParseWalkToKnownPathCommand(Player, moveDirections) ).Then( () =>
                    {
                        resolve();
                    } );
                }
            } );
        }
    }
}