using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class FollowCommand : Command
    {
        public FollowCommand(Player player, Creature target)
        {
            Player = player;

            Target = target;
        }

        public Player Player { get; set; }

        public Creature Target { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            if ( !Player.Tile.Position.IsInPlayerRange(Target.Tile.Position) )
            {
                //Act

                Player.AttackTarget = null;

                server.CancelQueueForExecution(Constants.PlayerAttackSchedulerEvent(Player) );

                Player.FollowTarget = null;

                server.CancelQueueForExecution(Constants.PlayerActionSchedulerEvent(Player) );

                //Notify

                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),
                        
                                                        new StopAttackAndFollowOutgoingPacket(0) );
            }
            else
            {
                if ( !Player.Tile.Position.IsNextTo(Target.Tile.Position) )
                {
                    //Act

                    //Notify

                    MoveDirection[] moveDirections = server.Pathfinding.GetMoveDirections(Player.Tile.Position, Target.Tile.Position);

                    if (moveDirections.Length != 0)
                    {
                        new CreatureMoveCommand(Player, server.Map.GetTile(Player.Tile.Position.Offset(moveDirections[0] ) ) ).Execute(server, context);
                    }
                }

                server.QueueForExecution(Constants.PlayerActionSchedulerEvent(Player), 1000 * Player.Tile.Ground.Metadata.Speed / Player.Speed, this);
            }
        }
    }
}