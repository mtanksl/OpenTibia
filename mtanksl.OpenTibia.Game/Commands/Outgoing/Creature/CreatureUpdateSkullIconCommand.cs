using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateSkullIconCommand : Command
    {
        public CreatureUpdateSkullIconCommand(Creature creature, SkullIcon skullIcon)
        {
            Creature = creature;

            SkullIcon = skullIcon;
        }

        public Creature Creature { get; set; }

        public SkullIcon SkullIcon { get; set; }

        public override Promise Execute()
        {
            if (Creature.SkullIcon != SkullIcon)
            {
                Creature.SkullIcon = SkullIcon;

                foreach (var observer in Context.Server.Map.GetObservers(Creature.Tile.Position).OfType<Player>() )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer.Client.Connection, new SetSkullIconOutgoingPacket(Creature.Id, Creature.SkullIcon) );

                        Context.AddEvent(observer, new CreatureUpdateSkullIconEventArgs(Creature, SkullIcon) );
                    }
                }

                Context.AddEvent(new CreatureUpdateSkullIconEventArgs(Creature, SkullIcon) );
            }

            return Promise.Completed;
        }
    }
}