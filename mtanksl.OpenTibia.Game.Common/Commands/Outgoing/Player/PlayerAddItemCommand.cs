using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class PlayerAddItemCommand : Command
    {
        public PlayerAddItemCommand(Player player, Item item)
        {
            Player = player;

            Item = item;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override Promise Execute()
        {
            uint weight = Item.GetWeight();
            
            uint capacity = Player.Capacity;

            if (weight <= capacity)
            {
                Container toContainer = Player.Inventory.GetContent( (byte)Slot.Backpack) as Container;

                if (toContainer != null)
                {
                    if (toContainer.Count < toContainer.Metadata.Capacity)
                    {
                        return Context.AddCommand(new ContainerAddItemCommand(toContainer, Item) );
                    }
                }

                toContainer = Player.Inventory.GetContent( (byte)Slot.Ammo) as Container;

                if (toContainer != null)
                {
                    if (toContainer.Count < toContainer.Metadata.Capacity)
                    {
                        return Context.AddCommand(new ContainerAddItemCommand(toContainer, Item) );
                    }
                }

                Item toItem = (Item)Player.Inventory.GetContent( (byte)Slot.Ammo);

                if (toItem == null)
                {
                    return Context.AddCommand(new InventoryAddItemCommand(Player.Inventory, (byte)Slot.Ammo, Item) );
                }
            }

            return Context.AddCommand(new TileAddItemCommand(Player.Tile, Item) );
        }
    }
}