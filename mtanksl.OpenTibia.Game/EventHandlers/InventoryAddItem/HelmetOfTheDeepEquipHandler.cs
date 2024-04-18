using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class HelmetOfTheDeepEquipHandler : EventHandlers.EventHandler<InventoryAddItemEventArgs>
    {
        private static HashSet<ushort> helmetOfTheDeeps = new HashSet<ushort>() { 5461 };

        public override Promise Handle(InventoryAddItemEventArgs e)
        {
            if (helmetOfTheDeeps.Contains(e.Item.Metadata.OpenTibiaId) )
            {
                if ( (Slot)e.Slot == Slot.Head)
                {
                    if (e.Inventory.Player.HasSpecialCondition(SpecialCondition.Drowning) )
                    {
                        return Context.AddCommand(new CreatureRemoveConditionCommand(e.Inventory.Player, ConditionSpecialCondition.Drowning) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}