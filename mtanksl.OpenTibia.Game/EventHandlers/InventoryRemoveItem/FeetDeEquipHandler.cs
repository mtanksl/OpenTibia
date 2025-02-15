using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FeetDeEquipHandler : EventHandlers.EventHandler<InventoryRemoveItemEventArgs>
    {
        private readonly Dictionary<ushort, ushort> feetDeEquip;

        public FeetDeEquipHandler()
        {
            feetDeEquip = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.feetDeEquip");
        }

        public override Promise Handle(InventoryRemoveItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (feetDeEquip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && (Slot)e.Slot == Slot.Feet)
            {
                return Context.AddCommand(new ItemTransformCommand(e.Item, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}