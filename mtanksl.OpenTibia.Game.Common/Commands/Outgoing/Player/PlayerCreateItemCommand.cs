using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class PlayerCreateItemCommand : Command
    {
        public PlayerCreateItemCommand(Player player, ushort openTibiaId, byte count)
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
            ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByOpenTibiaId(OpenTibiaId);

            uint weight;

            if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                weight = Count * (itemMetadata.Weight ?? 0);
            }
            else
            {
                weight = (itemMetadata.Weight ?? 0);
            }

            uint capacity = Player.Capacity;

            if (weight <= capacity)
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

                Item toItem = (Item)Player.Inventory.GetContent( (byte)Slot.Extra);

                if (toItem == null)
                {
                    return Context.AddCommand(new InventoryCreateItemCommand(Player.Inventory, (byte)Slot.Extra, OpenTibiaId, Count) );
                }
            }

            return Context.AddCommand(new TileCreateItemOrIncrementCommand(Player.Tile, OpenTibiaId, Count) );
        }
    }
}