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
            PlayerActionDelayBehaviour playerActionBehaviour = Context.Server.Components.GetComponent<PlayerActionDelayBehaviour>(Player);

            if (playerActionBehaviour != null)
            {
                Context.Server.Components.RemoveComponent(Player, playerActionBehaviour);
            }

            PlayerWalkDelayBehaviour playerWalkBehaviour = Context.Server.Components.GetComponent<PlayerWalkDelayBehaviour>(Player);

            if (playerWalkBehaviour != null)
            {
                if (Context.Server.Components.RemoveComponent(Player, playerWalkBehaviour) )
                {
                    Context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            PlayerAttackAndFollowExternalBehaviour playerAttackAndFollowBehaviour = Context.Server.Components.GetComponent<PlayerAttackAndFollowExternalBehaviour>(Player);

            if (playerAttackAndFollowBehaviour != null)
            {
                playerAttackAndFollowBehaviour.Stop();
            }

            Context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

            return Promise.Completed;
        }
    }
}