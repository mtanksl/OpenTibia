using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingDeEquipHandler : EventHandlers.EventHandler<InventoryRemoveItemEventArgs>
    {
        private static Dictionary<ushort, ushort> dequip = new Dictionary<ushort, ushort>()
        {
            { 2202, 2165 }, // Stealth ring
            { 2203, 2166 }, // Power ring
            { 2204, 2167 }, // Energy ring
            { 2205, 2168 }, // Life ring
            { 2206, 2169 }, // Time ring
            { 2210, 2207 }, // Sword ring
            { 2211, 2208 }, // Axe ring
            { 2212, 2209 }, // Club ring
            { 2213, 2210 }, // Dwarven ring
            { 2214, 2211 } // Ring of healing
        };

        public override Promise Handle(InventoryRemoveItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (dequip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if ( (Slot)e.Slot == Slot.Ring)
                {
                    return Context.AddCommand(new ItemTransformCommand(e.Item, toOpenTibiaId, 1) );
                }
            }

            return Promise.Completed;
        }
    }
}