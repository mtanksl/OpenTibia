using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class ItemSwapCommand : Command
    {
        public ItemSwapCommand(Item fromItem, Item toItem)
        {
            FromItem = fromItem;

            ToItem = toItem;
        }

        public Item FromItem { get; set; }

        public Item ToItem { get; set; }

        public byte ToIndex { get; set; }

        public override Promise Execute()
        {
            switch (ToItem.Parent)
            {
                case Inventory toInventory:

                    switch (FromItem.Parent)
                    {
                        case Tile fromTile:

                            return Context.AddCommand(new InventoryReplaceItemCommand(toInventory, ToItem, FromItem) ).Then( () =>
                            {
                                return Context.AddCommand(new TileAddItemCommand(fromTile, ToItem) );
                            } );

                        case Container fromContainer:

                            return Context.AddCommand(new InventoryReplaceItemCommand(toInventory, ToItem, FromItem) ).Then( () =>
                            {
                                return Context.AddCommand(new ContainerAddItemCommand(fromContainer, ToItem) );
                            } );

                        default:

                            throw new NotImplementedException();
                    }

                default:

                    throw new NotImplementedException();
            }
        }
    }
}