using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class InventoryCreateItemCommand : Command
    {
        public InventoryCreateItemCommand(Inventory inventory, byte slot, ushort openTibiaId, byte count)
        {
            Inventory = inventory;

            Slot = slot;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Item item = context.Server.ItemFactory.Create(OpenTibiaId);

            if (item is StackableItem stackableItem)
            {
                stackableItem.Count = Count;
            }
            else if (item is FluidItem fluidItem)
            {
                fluidItem.FluidType = (FluidType)Count;
            }

            Command command = context.TransformCommand(new InventoryAddItemCommand(Inventory, Slot, item) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}