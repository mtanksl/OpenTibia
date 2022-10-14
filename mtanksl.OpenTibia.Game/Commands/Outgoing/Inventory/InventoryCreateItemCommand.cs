using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class InventoryCreateItemCommand : CommandResult<Item>
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

        public override PromiseResult<Item> Execute(Context context)
        {
            return PromiseResult<Item>.Run(resolve =>
            {
                Item item = context.Server.ItemFactory.Create(OpenTibiaId, Count);

                if (item != null)
                {
                    context.AddCommand(new InventoryAddItemCommand(Inventory, Slot, item) ).Then(ctx =>
                    {
                        resolve(ctx, item);
                    } );
                }
            } );
        }
    }
}