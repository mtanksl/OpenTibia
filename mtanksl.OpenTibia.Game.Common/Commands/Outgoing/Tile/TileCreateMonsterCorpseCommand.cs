using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class TileCreateMonsterCorpseCommand : CommandResult<Item>
    {
        public TileCreateMonsterCorpseCommand(Tile tile, MonsterMetadata metadata)
        {
            Tile = tile;

            Metadata = metadata;
        }

        public Tile Tile { get; set; }

        public MonsterMetadata Metadata { get; set; }

        public override PromiseResult<Item> Execute()
        {
            return Context.AddCommand(new TileCreateItemCommand(Tile, Metadata.Corpse, 1) ).Then( (item) =>
            {
                if (item is Container container)
                {
                    if (Metadata.LootItems != null)
                    {
                        foreach (var lootItem in Metadata.LootItems)
                        {
                            if (Context.Server.Randomization.Take(1, lootItem.KillsToGetOne) == 1)
                            {
                                //TODO: CountMax for unstackable items

                                int total = Context.Server.Randomization.Take(lootItem.CountMin, lootItem.CountMax);

                                while (total > 0)
                                {
                                    int count = Math.Min(100, total);

                                    total -= count;

                                    Item loot = Context.Server.ItemFactory.Create(lootItem.OpenTibiaId, (byte)count);

                                    if (loot != null)
                                    {
                                        Context.Server.ItemFactory.Attach(loot);

                                        container.AddContent(loot);

                                        if (container.Count >= container.Metadata.Capacity)
                                        {
                                            total = -1;

                                            break;
                                        }
                                    }
                                }

                                if (total == -1)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                return Promise.FromResult(item);
            } );
        }
    }
}