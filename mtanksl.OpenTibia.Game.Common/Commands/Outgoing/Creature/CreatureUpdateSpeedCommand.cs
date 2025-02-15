using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateSpeedCommand : Command
    {
        public CreatureUpdateSpeedCommand(Creature creature, int conditionSpeed, int itemSpeed)
        {
            Creature = creature;

            ConditionSpeed = conditionSpeed;

            ItemSpeed = itemSpeed;
        }

        public Creature Creature { get; set; }

        public int ConditionSpeed { get; set; }

        public int ItemSpeed { get; set; }

        public override Promise Execute()
        {
            if (Creature.ConditionSpeed != ConditionSpeed || Creature.ItemSpeed != ItemSpeed)
            {
                Creature.ConditionSpeed = ConditionSpeed;
                Creature.ItemSpeed = ItemSpeed;

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                {
                    byte clientIndex;

                    if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                    {
                        Context.AddPacket(observer, new SetSpeedOutgoingPacket(Creature.Id, Creature.ClientSpeed) );
                    }
                }

                Context.AddEvent(new CreatureUpdateSpeedEventArgs(Creature, ConditionSpeed, ItemSpeed) );
            }

            return Promise.Completed;
        }
    }
}