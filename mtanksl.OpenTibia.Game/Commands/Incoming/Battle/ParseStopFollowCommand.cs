using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStopFollowCommand : Command
    {
        public ParseStopFollowCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

                AttackAndFollowBehaviour component = context.Server.Components.GetComponent<AttackAndFollowBehaviour>(Player);

                component.Stop();

                resolve(context);
            } );
        }
    }
}