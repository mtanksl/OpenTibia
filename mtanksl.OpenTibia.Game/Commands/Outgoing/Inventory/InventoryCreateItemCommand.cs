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

        public override PromiseResult<Item> Execute()
        {
            Item item = Context.Server.ItemFactory.Create(OpenTibiaId, Count);

            if (item != null)
            {
                return Context.AddCommand(new InventoryAddItemCommand(Inventory, Slot, item) ).Then( () =>
                {
                     return Promise.FromResult(item);
                } );
            }

            return Promise.FromResult(item);
        }
    }
}