using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class ItemExtensions
    {
        public static Promise Decay(this Item item, int executeInMilliseconds, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            return context.AddCommand(new ItemDecayCommand(item, executeInMilliseconds, openTibiaId, count) );
        }

        public static Promise Decrement(this Item item, byte count)
        {
            Context context = Context.Current;

            return context.AddCommand(new ItemDecrementCommand(item, count) );
        }

        public static Promise Destroy(this Item item)
        {
            Context context = Context.Current;

            return context.AddCommand(new ItemDestroyCommand(item) );
        }

        public static Promise Move(this Item item, Tile toTile)
        {
            Context context = Context.Current;

            return context.AddCommand(new ItemMoveToTileCommand(item, toTile) );
        }

        public static Promise Move(this Item item, Inventory toInventory, byte slot)
        {
            Context context = Context.Current;

            return context.AddCommand(new ItemMoveToInventoryCommand(item, toInventory, slot) );
        }

        public static Promise Move(this Item item, Container toContiner)
        {
            Context context = Context.Current;

            return context.AddCommand(new ItemMoveToContainerCommand(item, toContiner) );
        }

        public static PromiseResult<Item> Transform(this Item item, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;
            
            return context.AddCommand(new ItemTransformCommand(item, openTibiaId, count) );
        }
    }
}