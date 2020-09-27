using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class InventoryDestroyItemCommand : Command
    {
        public InventoryDestroyItemCommand(Inventory inventory, Item item)
        {
            Inventory = inventory;

            Item = item;
        }

        public Inventory Inventory { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new InventoryRemoveItemCommand(Inventory, Item) );

            command.Completed += (s, e) =>
            {
                e.Context.Server.ItemFactory.Destroy(Item);

                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}