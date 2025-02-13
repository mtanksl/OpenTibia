using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateSpeedCommand : Command
    {
        public CreatureUpdateSpeedCommand(Creature creature, ushort speed)
        {
            Creature = creature;

            Speed = speed;
        }

        public Creature Creature { get; set; }

        public ushort Speed { get; set; }

        public override Promise Execute()
        {
            if (Creature.Speed != Speed)
            {
                //TODO: Custom stats
                //Creature.Speed = Speed;

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer, new SetSpeedOutgoingPacket(Creature.Id, Creature.Speed) );
                    }
                }

                Context.AddEvent(new CreatureUpdateSpeedEventArgs(Creature, Speed) );
            }

            return Promise.Completed;
        }
    }
}