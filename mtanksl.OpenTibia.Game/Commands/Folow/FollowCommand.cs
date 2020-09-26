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

        public override void Execute(Context context)
        {
            //Arrange

            if ( !Player.Tile.Position.IsInBattleRange(Target.Tile.Position) )
            {
                //Act

                Player.AttackTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                Player.FollowTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                //Notify

                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),
                        
                                                        new StopAttackAndFollowOutgoingPacket(0) );
            }
            else
            {
                if ( !Player.Tile.Position.IsNextTo(Target.Tile.Position) )
                {
                    //Act

                    //Notify

                    MoveDirection[] moveDirections = context.Server.Pathfinding.GetMoveDirections(Player.Tile.Position, Target.Tile.Position);

                    if (moveDirections.Length != 0)
                    {
                        new CreatureMoveCommand(Player, context.Server.Map.GetTile(Player.Tile.Position.Offset(moveDirections[0] ) ) ).Execute(context);
                    }
                }

                context.Server.QueueForExecution(Constants.CreatureAttackSchedulerEvent(Player), 1000 * Player.Tile.Ground.Metadata.Speed / Player.Speed, this);
            }
        }
    }
}