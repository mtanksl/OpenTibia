using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class TileExtensions
    {
        public static PromiseResult<byte> AddCreature(this Tile tile, Creature creature)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileAddCreatureCommand(tile, creature) );
        }

        public static PromiseResult<byte> AddItem(this Tile tile, Item item)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileAddItemCommand(tile, item) );
        }

        public static PromiseResult<Item> CreateItem(this Tile tile, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileCreateItemCommand(tile, openTibiaId, count) );
        }

        public static PromiseResult<Monster> CreateMonster(this Tile tile, string name)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileCreateMonsterCommand(tile, name) );
        }

        public static PromiseResult<Npc> CreateNpc(this Tile tile, string name)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileCreateNpcCommand(tile, name) );
        }

        public static PromiseResult<byte> RemoveCreature(this Tile tile, Creature creature)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileRemoveCreatureCommand(tile, creature) );
        }

        public static PromiseResult<byte> RemoveItem(this Tile tile, Item item)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileRemoveItemCommand(tile, item) );
        }

        public static PromiseResult<byte> ReplaceItem(this Tile tile, Item fromItem, Item toItem)
        {
            Context context = Context.Current;

            return context.AddCommand(new TileReplaceItemCommand(tile, fromItem, toItem) );
        }
    }
}