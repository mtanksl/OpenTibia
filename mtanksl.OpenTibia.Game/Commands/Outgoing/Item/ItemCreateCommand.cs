using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemCreateCommand : Command
    {
        public ItemCreateCommand(Tile tile, ushort openTibiaId, byte count)
        {
            Tile = tile;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public ItemCreateCommand(Inventory inventory, byte slot, ushort openTibiaId, byte count)
        {
            Inventory = inventory;

            Slot = slot;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public ItemCreateCommand(Container container, ushort openTibiaId, byte count)
        {
            Container = container;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Tile Tile { get; set; }

        public Inventory Inventory { get; set; }

        public byte Slot { get; set; }

        public Container Container { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Item item = context.Server.ItemFactory.Create(OpenTibiaId, Count);

            if (item != null)
            {
                if (Tile != null)
                {
                    ItemCreateTile(context, Tile, item);
                }
                else if (Inventory != null)
                {
                    ItemCreateInventory(context, Inventory, Slot, item);
                }
                else if (Container != null)
                {
                    ItemCreateContainer(context, Container, item);
                }
            }
        }

        private void ItemCreateTile(Context context, Tile tile, Item item)
        {
            if (item is StackableItem fromStackableItem && tile.TopItem != null && tile.TopItem is StackableItem toStackableItem && fromStackableItem.Metadata.OpenTibiaId == toStackableItem.Metadata.OpenTibiaId)
            {
                int sum = fromStackableItem.Count + toStackableItem.Count;

                if (sum > 100)
                {
                    context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, 100) ).Then(ctx =>
                    {
                        fromStackableItem.Count = (byte)(sum - 100);

                        return ctx.AddCommand(new TileAddItemCommand(Tile, item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );
                }
                else
                {
                    context.AddCommand(new StackableItemUpdateCountCommand(toStackableItem, (byte)(fromStackableItem.Count + toStackableItem.Count) ) ).Then(ctx =>
                    {
                        fromStackableItem.Count = 0;

                        return ctx.AddCommand(new ItemDestroyCommand(item) );

                    } ).Then(ctx =>
                    {
                        OnComplete(ctx);
                    } );
                }
            }
            else
            {
                context.AddCommand(new TileAddItemCommand(tile, item) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
        }

        private void ItemCreateInventory(Context context, Inventory inventory, byte slot, Item item)
        {
            Item topItem = (Item)inventory.GetContent(slot);

            if (topItem != null)
            {
                ItemCreateTile(context, inventory.Player.Tile, item);
            }
            else
            {
                context.AddCommand(new InventoryAddItemCommand(inventory, slot, item) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
        }

        private void ItemCreateContainer(Context context, Container container, Item item)
        {
            if (container.Count == container.Metadata.Capacity)
            {
                switch (Container.Root() )
                {
                    case Tile tile:

                        ItemCreateTile(context, tile, item);

                        break;

                    case Inventory inventory:

                        ItemCreateTile(context, inventory.Player.Tile, item);

                        break;
                }
            }
            else
            {
                context.AddCommand(new ContainerAddItemCommand(Container, item) ).Then(ctx =>
                {
                    OnComplete(ctx);
                } );
            }
        }
    }
}