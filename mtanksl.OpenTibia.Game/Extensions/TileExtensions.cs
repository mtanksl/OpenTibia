using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class TileExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddCreature(this Tile tile, Creature creature)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileAddCreatureCommand(tile, creature) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddItem(this Tile tile, Item item)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileAddItemCommand(tile, item) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static PromiseResult<Item> CreateItem(this Tile tile, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileCreateItemCommand(tile, openTibiaId, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static PromiseResult<Monster> CreateMonster(this Tile tile, string name)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileCreateMonsterCommand(tile, name) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static PromiseResult<Npc> CreateNpc(this Tile tile, string name)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileCreateNpcCommand(tile, name) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise IncrementOrCreateItem(this Tile tile, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileIncrementOrCreateItemCommand(tile, openTibiaId, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise RemoveCreature(this Tile tile, Creature creature)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileRemoveCreatureCommand(tile, creature) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise RemoveItem(this Tile tile, Item item)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileRemoveItemCommand(tile, item) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise ReplaceItem(this Tile tile, Item fromItem, Item toItem)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new TileReplaceItemCommand(tile, fromItem, toItem) );
        }
    }
}