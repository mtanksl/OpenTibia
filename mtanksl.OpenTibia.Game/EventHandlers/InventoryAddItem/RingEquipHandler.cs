using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingEquipHandler : EventHandlers.EventHandler<InventoryAddItemEventArgs>
    {
        private readonly Dictionary<ushort, ushort> ringEquip;

        public RingEquipHandler()
        {
            ringEquip = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.ringEquip");
        }

        public override Promise Handle(InventoryAddItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (ringEquip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
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