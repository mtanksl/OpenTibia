using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class StopFollowCommand : Command
    {
        public StopFollowCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override void Execute(Context context)
        {
            if (Player.FollowTarget != null)
            {
                Player.AttackTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackOrFollowSchedulerEvent(Player) );

                Player.FollowTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackOrFollowSchedulerEvent(Player) );

                context.WritePacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );
            }

            base.OnCompleted(context);
        }
    }
}