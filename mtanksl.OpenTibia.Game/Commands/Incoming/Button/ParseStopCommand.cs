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
            AttackAndFollowBehaviour component = Context.Server.Components.GetComponent<AttackAndFollowBehaviour>(Player);

            component.Stop();

            Context.Server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(Player) );

            if (Context.Server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(Player) ) )
            {
                Context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
            }

            Context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

            return Promise.Completed;
        }
    }
}