using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class PlayerCreateItemCommand : Command
    {
        public PlayerCreateItemCommand(Player player, ushort openTibiaId, byte typeCount)
        {
            Player = player;

            OpenTibiaId = openTibiaId;

            TypeCount = typeCount;
        }

        public Player Player { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte TypeCount { get; set; }

        public override Promise Execute()
        {
            ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByOpenTibiaId(OpenTibiaId);

            uint weight;

            if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                weight = TypeCount * (itemMetadata.Weight ?? 0);
            }
            else
            {
                weight = (itemMetadata.Weight ?? 0);
            }

            uint capacity = Player.Capacity;

            if (weight <= capacity)
            {
                Container toContainer = Player.Inventory.GetContent( (byte)Slot.Backpack) as Container;

                if (toContainer != null)
                {
                    if (toContainer.Count < toContainer.Metadata.Capacity)
                    {
                        return Context.AddCommand(new ContainerCreateItemCommand(toContainer, OpenTibiaId, TypeCount) );
                    }
                }

                toContainer = Player.Inventory.GetContent( (byte)Slot.Ammo) as Container;

                if (toContainer != null)
                {
                    if (toContainer.Count < toContainer.Metadata.Capacity)
                    {
                        return Context.AddCommand(new ContainerCreateItemCommand(toContainer, OpenTibiaId, TypeCount) );
                    }
                }

                Item toItem = (Item)Player.Inventory.GetContent( (byte)Slot.Ammo);

                if (toItem == null)
                {
                    return Context.AddCommand(new InventoryCreateItemCommand(Player.Inventory, (byte)Slot.Ammo, OpenTibiaId, TypeCount) );
                }
            }

            return Context.AddCommand(new TileCreateItemOrIncrementCommand(Player.Tile, OpenTibiaId, TypeCount) );
        }
    }
}