using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RingDeEquipHandler : EventHandlers.EventHandler<InventoryRemoveItemEventArgs>
    {
        private readonly Dictionary<ushort, ushort> ringDequip;

        public RingDeEquipHandler()
        {
            ringDequip = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.ringDequip");
        }

        public override Promise Handle(InventoryRemoveItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (ringDequip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
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