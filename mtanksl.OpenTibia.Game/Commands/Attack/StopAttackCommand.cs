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

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            if (Player.AttackTarget != null)
            {
                //Act

                Player.AttackTarget = null;

                Player.FollowTarget = null;

                //Notify

                context.Write(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

                base.Execute(server, context);
            }
        }
    }
}