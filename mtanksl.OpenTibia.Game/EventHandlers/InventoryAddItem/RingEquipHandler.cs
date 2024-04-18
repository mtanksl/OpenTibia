using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingEquipHandler : EventHandlers.EventHandler<InventoryAddItemEventArgs>
    {
        private static Dictionary<ushort, ushort> equip = new Dictionary<ushort, ushort>()
        {
            { 2165, 2202 }, // Stealth ring
            { 2166, 2203 }, // Power ring
            { 2167, 2204 }, // Energy ring
            { 2168, 2205 }, // Life ring
            { 2169, 2206 }, // Time ring
            { 2207, 2210 }, // Sword ring
            { 2208, 2211 }, // Axe ring
            { 2209, 2212 }, // Club ring
            { 2210, 2213 }, // Dwarven ring
            { 2211, 2214 } // Ring of healing
        };

        public override Promise Handle(InventoryAddItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (equip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
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