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
                
        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                AttackAndFollowBehaviour component = Context.Server.Components.GetComponent<AttackAndFollowBehaviour>(Player);

                component.Stop();

                Context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

                if (Context.Server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) ) )
                {
                    Context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
                }

                Context.Server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(Player) );

                resolve();
            } );
        }
    }
}