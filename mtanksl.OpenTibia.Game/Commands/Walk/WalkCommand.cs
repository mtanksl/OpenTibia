using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class WalkCommand : Command
    {
        public WalkCommand(Player player, MoveDirection moveDirection)
        {
            Player = player;

            MoveDirection = moveDirection;
        }

        public Player Player { get; set; }

        public MoveDirection MoveDirection { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = Player.Tile;

            Position fromPosition = fromTile.Position;

            Position toPosition = fromPosition.Offset(MoveDirection);

            Tile toTile = server.Map.GetTile(toPosition);

            if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
            }
            else
            {
                TeleportCommand command = new TeleportCommand(Player, toPosition);

                command.Completed += (s, e) =>
                {
                    //Act

                    base.Execute(e.Server, e.Context);
                };

                server.QueueForExecution(Constants.PlayerWalkSchedulerEvent(Player), 1000 * fromTile.Ground.Metadata.Speed / Player.Speed, command);
            }
        }
    }
}