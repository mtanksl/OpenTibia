using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
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
            PlayerWalkDelayBehaviour creatureWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(Player);

            if (creatureWalkDelayBehaviour != null)
            {
                if (Context.Server.GameObjectComponents.RemoveComponent(Player, creatureWalkDelayBehaviour) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                }
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