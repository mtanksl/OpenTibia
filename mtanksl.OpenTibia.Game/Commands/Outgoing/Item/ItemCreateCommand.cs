using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ItemCreateCommand : CommandResult<Item>
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
            Item item = context.Server.ItemFactory.Create(OpenTibiaId);

            if (item is StackableItem stackableItem)
            {
                stackableItem.Count = Count;
            }
            else if (item is FluidItem fluidItem)
            {
                fluidItem.FluidType = (FluidType)Count;
            }

            if (item != null)
            {
                if (Tile != null)
                {
                    context.AddCommand(new TileAddItemCommand(Tile, item) ).Then(ctx =>
                    {
                        OnComplete(ctx, item);
                    } );
                }
                else if (Inventory != null)
                {
                    context.AddCommand(new InventoryAddItemCommand(Inventory, Slot, item) ).Then(ctx =>
                    {
                        OnComplete(ctx, item);
                    } );
                }
                else if (Container != null)
                {
                    context.AddCommand(new ContainerAddItemCommand(Container, item) ).Then(ctx =>
                    {
                        OnComplete(ctx, item);
                    } );
                }
            }
        }
    }
}