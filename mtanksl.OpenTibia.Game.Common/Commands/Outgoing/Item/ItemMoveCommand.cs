using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ItemMoveCommand : Command
    {
        public ItemMoveCommand(Item item, IContainer toContainer, byte toIndex)
        {
            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;
        }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public override Promise Execute()
        {
            List<Promise> promises = new List<Promise>();

            switch (Item.Parent)
            {
                case Tile fromTile:

                    promises.Add(Context.AddCommand(new TileRemoveItemCommand(fromTile, Item) ) );

                    break;

                case Inventory fromInventory:

                    promises.Add(Context.AddCommand(new InventoryRemoveItemCommand(fromInventory, Item) ) );

                    break;

                case Container fromContainer:

                    promises.Add(Context.AddCommand(new ContainerRemoveItemCommand(fromContainer, Item) ) );

                    break;
            }

            switch (ToContainer)
            {
                case Tile toTile:

                    promises.Add(Context.AddCommand(new TileAddItemCommand(toTile, Item) ) );

                    break;

                case Inventory toInventory:

                    promises.Add(Context.AddCommand(new InventoryAddItemCommand(toInventory, ToIndex, Item) ) );

                    break;

                case Container toContainer:

                    promises.Add(Context.AddCommand(new ContainerAddItemCommand(toContainer, Item) ) );
                        
                    break;
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}
