using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

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

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer, new ThingUpdateOutgoingPacket(Creature.Tile.Position, clientIndex, Creature.Id, Creature.Direction) );
                    }
                }

                Context.AddEvent(new CreatureUpdateDirectionEventArgs(Creature, Direction) );
            }

            return Promise.Completed;
        }
    }
}