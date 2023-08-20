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
                
        public override Promise Execute()
        {
            PlayerAttackAndFollowBehaviour playerAttackAndFollowBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerAttackAndFollowBehaviour>(Player);

            if (playerAttackAndFollowBehaviour != null)
            {
                playerAttackAndFollowBehaviour.StopAttackAndFollow();
            }

            Context.AddPacket(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );

            return Promise.Completed;
        }
    }
}