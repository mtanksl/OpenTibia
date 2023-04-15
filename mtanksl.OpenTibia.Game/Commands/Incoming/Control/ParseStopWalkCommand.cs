using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStopWalkCommand : Command
    {
        public ParseStopWalkCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            PlayerActionBehaviour playerActionBehaviour = Context.Server.Components.GetComponent<PlayerActionBehaviour>(Player);

            if (playerActionBehaviour != null)
            {
                Context.Server.Components.RemoveComponent(Player, playerActionBehaviour);
            }

            PlayerWalkBehaviour playerWalkBehaviour = Context.Server.Components.GetComponent<PlayerWalkBehaviour>(Player);

            if (playerWalkBehaviour != null)
            {
                if (Context.Server.Components.RemoveComponent(Player, playerWalkBehaviour) )
                {
                    Context.AddPacket(Player.Client.Connection, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            return Promise.Completed;
        }
    }
}