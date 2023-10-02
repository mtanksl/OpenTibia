using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private Dictionary<ushort, ushort> equip = new Dictionary<ushort, ushort>()
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

        private Dictionary<ushort, ushort> dequip = new Dictionary<ushort, ushort>()
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

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.ToContainer is Inventory toInventory && (Slot)command.ToIndex == Slot.Ring)
            {
                ushort toOpenTibiaId;

                if (equip.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return next().Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                    } );
                }
            }
            else if (command.Item.Parent is Inventory fromInventory && (Slot)fromInventory.GetIndex(command.Item) == Slot.Ring)
            {
                ushort toOpenTibiaId;

                if (dequip.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
                {
                    return next().Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                    } );
                }
            }

            return next();
        }
    }
}