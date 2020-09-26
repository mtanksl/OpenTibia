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
            //Arrange

            if (Player.AttackTarget != null)
            {
                //Act

                Player.AttackTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                Player.FollowTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                //Notify

                context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );
            }

            base.Execute(context);
        }
    }
}