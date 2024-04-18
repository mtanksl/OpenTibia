using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseTurnCommand : IncomingCommand
    {
        public ParseTurnCommand(Player player, Direction direction)
        {
            Player = player;

            Direction = direction;
        }

        public Player Player { get; set; }

        public Direction Direction { get; set; }

        public override Promise Execute()
        {
            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                playerIdleBehaviour.SetLastAction();
            }

            PlayerWalkDelayBehaviour creatureWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(Player);

            if (creatureWalkDelayBehaviour != null)
            {
                if (Context.Server.GameObjectComponents.RemoveComponent(Player, creatureWalkDelayBehaviour) )
                {
                    Context.AddPacket(Player, new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            return Context.AddCommand(new CreatureUpdateDirectionCommand(Player, Direction) );
        }
    }
}