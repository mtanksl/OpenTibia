using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStopCommand : Command
    {
        public ParseStopCommand(Player player)
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

                if (context.Server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) ) )
                {
                    context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
                }

                context.Server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(Player) );

                resolve(context);
            } );
        }
    }
}