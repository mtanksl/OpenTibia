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

                Player.FollowTarget = null;

                context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );
            }

            OnComplete(context);
        }
    }
}