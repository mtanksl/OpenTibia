using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStopAttackCommand : Command
    {
        public ParseStopAttackCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
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