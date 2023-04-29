using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateSpeedCommand : Command
    {
        public CreatureUpdateSpeedCommand(Creature creature, ushort baseSpeed, ushort speed)
        {
            Creature = creature;

            BaseSpeed = baseSpeed;

            Speed = speed;
        }

        public Creature Creature { get; set; }

        public ushort BaseSpeed { get; set; }

        public ushort Speed { get; set; }

        public override Promise Execute()
        {
            if (Creature.BaseSpeed != BaseSpeed || Creature.Speed != Speed)
            {
                Creature.BaseSpeed = BaseSpeed;

                Creature.Speed = Speed;

                foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer.Client.Connection, new SetSpeedOutgoingPacket(Creature.Id, Creature.Speed) );

                        Context.AddEvent(observer, new CreatureUpdateSpeedEventArgs(Creature, BaseSpeed, Speed) );
                    }
                }

                Context.AddEvent(new CreatureUpdateSpeedEventArgs(Creature, BaseSpeed, Speed) );
            }

            return Promise.Completed;
        }
    }
}