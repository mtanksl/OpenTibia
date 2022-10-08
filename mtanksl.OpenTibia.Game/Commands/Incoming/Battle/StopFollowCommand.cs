using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class StopFollowCommand : Command
    {
        public StopFollowCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (Player.FollowTarget != null)
                {
                    Player.AttackTarget = null;

                    Player.FollowTarget = null;

                    context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );
                }

                resolve(context);
            } );
        }
    }
}