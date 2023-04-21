using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ItemTransformCommand : CommandResult<Item>
    {
        public ItemTransformCommand(Item fromItem, ushort openTibiaId, byte count)
        {
            FromItem = fromItem;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Item FromItem { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override PromiseResult<Item> Execute()
        {
            Item toItem = Context.Server.ItemFactory.Create(OpenTibiaId, Count);

            if (toItem != null)
            {
                switch (FromItem.Parent)
                {
                    case Tile tile:

                        return Context.AddCommand(new TileReplaceItemCommand(tile, FromItem, toItem) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(FromItem) );

                        } ).Then( () =>
                        {
                            return Promise.FromResult(toItem);
                        } );

                    case Inventory inventory:

                        return Context.AddCommand(new InventoryReplaceItemCommand(inventory, FromItem, toItem) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(FromItem) );

                        } ).Then( () =>
                        {
                            return Promise.FromResult(toItem);
                        } );

                    case Container container:

                        return Context.AddCommand(new ContainerReplaceItemCommand(container, FromItem, toItem) ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(FromItem) );

                        } ).Then( () =>
                        {
                            return Promise.FromResult(toItem);
                        } );                 
                }
            }

            return Promise.FromResult(toItem);           
        }
    }
}