using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class ParseStopAttackCommand : Command
    {
        public ParseStopAttackCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

                AttackAndFollowBehaviour component = Player.GetComponent<AttackAndFollowBehaviour>();

                component.Stop();

                resolve(context);
            } );
        }
    }
}