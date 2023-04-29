using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateLightCommand : Command
    {
        public CreatureUpdateLightCommand(Creature creature, Light light)
        {
            Creature = creature;

            Light = light;
        }

        public Creature Creature { get; set; }

        public Light Light { get; set; }

        public override Promise Execute()
        {
            if (Creature.Light != Light)
            {
                Creature.Light = Light;

                foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer.Client.Connection, new SetLightOutgoingPacket(Creature.Id, Creature.Light) );
                    }
                }

                Context.AddEvent(new CreatureUpdateLightEventArgs(Creature, Light) );
            }

            return Promise.Completed;
        }
    }
}