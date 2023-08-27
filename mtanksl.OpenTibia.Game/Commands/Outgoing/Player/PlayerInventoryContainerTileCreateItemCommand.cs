using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class PlayerInventoryContainerTileCreateItemCommand : Command
    {
        public PlayerInventoryContainerTileCreateItemCommand(Player player, ushort openTibiaId, byte count)
        {
            Player = player;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Player Player { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Container toContainer = Player.Inventory.GetContent( (byte)Slot.Container) as Container;

            if (toContainer != null)
            {
                if (toContainer.Count < toContainer.Metadata.Capacity)
                {
                    return Context.AddCommand(new ContainerCreateItemCommand(toContainer, OpenTibiaId, Count) );
                }
            }

            toContainer = Player.Inventory.GetContent( (byte)Slot.Extra) as Container;

            if (toContainer != null)
            {
                if (toContainer.Count < toContainer.Metadata.Capacity)
                {
                    return Context.AddCommand(new ContainerCreateItemCommand(toContainer, OpenTibiaId, Count) );
                }
            }

            Item toItem = Player.Inventory.GetContent( (byte)Slot.Extra) as Item;

            if (toItem == null)
            {
                return Context.AddCommand(new InventoryCreateItemCommand(Player.Inventory, (byte)Slot.Extra, OpenTibiaId, Count) );
            }

            return Context.AddCommand(new TileIncrementOrCreateItemCommand(Player.Tile, OpenTibiaId, Count) );
        }
    }
}