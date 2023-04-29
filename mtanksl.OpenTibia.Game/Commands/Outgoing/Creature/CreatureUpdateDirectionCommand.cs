using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateDirectionCommand : Command
    {
        public CreatureUpdateDirectionCommand(Creature creature, Direction direction)
        {
            Creature = creature;

            Direction = direction;
        }

        public Creature Creature { get; set; }

        public Direction Direction { get; set; }

        public override Promise Execute()
        {
            if (Creature.Direction != Direction)
            {
                Creature.Direction = Direction;

                foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
                {
                    if (observer == Creature)
                    {
                        PlayerActionDelayBehaviour playerActionDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerActionDelayBehaviour>(observer);

                        if (playerActionDelayBehaviour != null)
                        {
                            Context.Server.GameObjectComponents.RemoveComponent(observer, playerActionDelayBehaviour);
                        }

                        PlayerWalkDelayBehaviour playerWalkDelayBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerWalkDelayBehaviour>(observer);

                        if (playerWalkDelayBehaviour != null)
                        {
                            if (Context.Server.GameObjectComponents.RemoveComponent(observer, playerWalkDelayBehaviour) )
                            {
                                Context.AddPacket(observer.Client.Connection, new StopWalkOutgoingPacket(observer.Direction) );
                            }
                        }
                    }

                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer.Client.Connection, new ThingUpdateOutgoingPacket(Creature.Tile.Position, clientIndex, Creature.Id, Creature.Direction) );

                        Context.AddEvent(observer, new CreatureUpdateDirectionEventArgs(Creature, Direction) );
                    }
                }

                Context.AddEvent(new CreatureUpdateDirectionEventArgs(Creature, Direction) );
            }

            return Promise.Completed;
        }
    }
}