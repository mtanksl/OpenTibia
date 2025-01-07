using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class TileCreateMonsterCorpseCommand : CommandResult<Item>
    {
        private readonly ushort goldCoin;
        private readonly ushort platinumCoin;
        private readonly ushort crystalCoin;

        public TileCreateMonsterCorpseCommand(Tile tile, MonsterMetadata metadata)
        {
            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            crystalCoin = Context.Server.Values.GetUInt16("values.items.crystalCoin");

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
                            if (Context.Server.Randomization.HasProbability( (double)Context.Server.Config.GameplayLootRate / lootItem.KillsToGetOne) )
                            {
                                int min = 1;

                                int max = 1;

                                if (lootItem.OpenTibiaId == goldCoin || lootItem.OpenTibiaId == platinumCoin || lootItem.OpenTibiaId == crystalCoin)
                                {
                                    max *= Context.Server.Config.GameplayMoneyRate;
                                }
                                else
                                {
                                    ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByOpenTibiaId(lootItem.OpenTibiaId);

                                    if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
                                    {
                                        min = lootItem.CountMin;

                                        max = lootItem.CountMax;
                                    }
                                }

                                int total = Context.Server.Randomization.Take(min, max);

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