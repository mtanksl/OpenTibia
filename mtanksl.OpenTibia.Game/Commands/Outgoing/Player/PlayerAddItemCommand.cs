using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

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
            Container toContainer = Player.Inventory.GetContent( (byte)Slot.Container) as Container;

            if (toContainer != null)
            {
                if (toContainer.Count < toContainer.Metadata.Capacity)
                {
                    return Context.AddCommand(new ContainerAddItemCommand(toContainer, Item) );
                }
            }

            toContainer = Player.Inventory.GetContent( (byte)Slot.Extra) as Container;

            if (toContainer != null)
            {
                if (toContainer.Count < toContainer.Metadata.Capacity)
                {
                    return Context.AddCommand(new ContainerAddItemCommand(toContainer, Item) );
                }
            }

            Item toItem = Player.Inventory.GetContent( (byte)Slot.Extra) as Item;

            if (toItem == null)
            {
                return Context.AddCommand(new InventoryAddItemCommand(Player.Inventory, (byte)Slot.Extra, Item) );
            }

            return Context.AddCommand(new TileAddItemCommand(Player.Tile, Item) );
        }
    }
}