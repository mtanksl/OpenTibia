using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FeetEquipHandler : EventHandlers.EventHandler<InventoryAddItemEventArgs>
    {
        private readonly Dictionary<ushort, ushort> feetEquip;
        
        public FeetEquipHandler()
        {
            feetEquip = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.feetEquip");
        }

        public override Promise Handle(InventoryAddItemEventArgs e)
        {
            ushort toOpenTibiaId;

            if (feetEquip.TryGetValue(e.Item.Metadata.OpenTibiaId, out toOpenTibiaId) && (Slot)e.Slot == Slot.Feet)
            {
                return Context.AddCommand(new ItemTransformCommand(e.Item, toOpenTibiaId, 1) );
            }

            return Promise.Completed;
        }
    }
}