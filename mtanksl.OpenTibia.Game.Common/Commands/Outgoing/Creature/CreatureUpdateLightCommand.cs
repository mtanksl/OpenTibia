using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateLightCommand : Command
    {
        public CreatureUpdateLightCommand(Creature creature, Light conditionLight, Light itemLight)
        {
            Creature = creature;

            ConditionLight = conditionLight;

            ItemLight = itemLight;
        }

        public Creature Creature { get; set; }

        public Light ConditionLight { get; set; }

        public Light ItemLight { get; set; }

        public override Promise Execute()
        {
            if (Creature.ConditionLight != ConditionLight || Creature.ItemLight != ItemLight)
            {
                Creature.ConditionLight = ConditionLight; 
                Creature.ItemLight = ItemLight;

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer, new SetLightOutgoingPacket(Creature.Id, Creature.ClientLight) );
                    }
                }

                Context.AddEvent(new CreatureUpdateLightEventArgs(Creature, ConditionLight, ItemLight) );
            }

            return Promise.Completed;
        }
    }
}