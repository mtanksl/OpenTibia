using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class StopAttackCommand : Command
    {
        public StopAttackCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            if (Player.AttackTarget != null)
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