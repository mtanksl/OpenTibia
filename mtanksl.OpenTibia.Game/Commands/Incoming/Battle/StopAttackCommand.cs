using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class StopAttackCommand : Command
    {
        public StopAttackCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (Player.AttackTarget != null)
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