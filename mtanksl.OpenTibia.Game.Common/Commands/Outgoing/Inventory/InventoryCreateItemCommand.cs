using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class InventoryCreateItemCommand : CommandResult<Item>
    {
        public InventoryCreateItemCommand(Inventory inventory, byte slot, ushort openTibiaId, byte typeCount)
        {
            Inventory = inventory;

            Slot = slot;

            OpenTibiaId = openTibiaId;

            TypeCount = typeCount;
        }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte TypeCount { get; set; }

        public override PromiseResult<Item> Execute()
        {
            Item item = Context.Server.ItemFactory.Create(OpenTibiaId, TypeCount);

            if (item != null)
            {
                Context.Server.ItemFactory.Attach(item);

                return Context.AddCommand(new InventoryAddItemCommand(Inventory, Slot, item) ).Then( () =>
                {
                     return Promise.FromResult(item);
                } );
            }

            return Promise.FromResult(item);
        }
    }
}