using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStopCommand : IncomingCommand
    {
        public ParseStopCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            PlayerWalkingDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkingDelayBehaviour>(Player);

            if (playerWalkDelayBehaviour != null)
            {
                if (Context.Server.GameObjectComponents.RemoveComponent(Player, playerWalkDelayBehaviour) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            PlayerWalkedDelayBehaviour playerActionDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkedDelayBehaviour>(Player);

            if (playerActionDelayBehaviour != null)
            {
                Context.Server.GameObjectComponents.RemoveComponent(Player, playerActionDelayBehaviour);
            }

            PlayerThinkBehaviour playerThinkBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerThinkBehaviour>(Player);

            if (playerThinkBehaviour != null)
            {
                playerThinkBehaviour.StopAttackAndFollow();
            }

            Context.AddPacket(Player, new StopAttackAndFollowOutgoingPacket(0) );

            return Promise.Completed;
        }
    }
}