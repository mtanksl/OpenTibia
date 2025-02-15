using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingDeEquipHandler : EventHandlers.EventHandler<InventoryRemoveItemEventArgs>
    {
        private readonly Dictionary<ushort, ushort> ringDeEquip;

        public RingDeEquipHandler()
        {
            ringDeEquip = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.ringDeEquip");
        }

        public override Promise Handle(InventoryRemoveItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (ringDeEquip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && (Slot)e.Slot == Slot.Ring)
            {
                return Context.AddCommand(new ItemTransformCommand(e.Item, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}