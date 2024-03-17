using OpenTibia.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class PlayerRemoveMoneyCommand : CommandResult<bool>
    {
        public PlayerRemoveMoneyCommand(Player player, int price)
        {
            Player = player;

            Price = price;
        }

        public Player Player { get; set; }

        public int Price { get; set; }

        public override async PromiseResult<bool> Execute()
        {
            int crystal = 0;

            int platinum = 0;

            int gold = 0;

            List<Item> crystals = new List<Item>();

            List<Item> platinums = new List<Item>();

            List<Item> golds = new List<Item>();

            int sum = Sum(Player.Inventory, crystals, platinums, golds);

            if (sum < Price)
            {
                return false;
            }

            int sumCrystal = crystals.Sum(i => ( (StackableItem)i).Count);

            int sumPlatinum = platinums.Sum(i => ( (StackableItem)i).Count);

            int sumGold = golds.Sum(i => ( (StackableItem)i).Count);

            while (Price > 0)
            {
                var n = Math.Min(Price / 10000, sumCrystal);

                if (n > 0)
                {
                    Price -= n * 10000;

                    sumCrystal -= n;

                    crystal -= n;
                }

                n = Math.Min(Price / 100, sumPlatinum);

                if (n > 0)
                {
                    Price -= n * 100;

                    sumPlatinum -= n;

                    platinum -= n;
                }

                n = Math.Min(Price / 1, sumGold);

                if (n > 0)
                {
                    Price -= n * 1;

                    sumGold -= n;

                    gold -= n;
                }

                if (Price > 0)
                {
                    if (sumPlatinum > 0)
                    {
                        sumPlatinum -= 1;

                        platinum -= 1;

                        sumGold += 100;

                        gold += 100;
                    }
                    else if (sumCrystal > 0)
                    {
                        sumCrystal -= 1;

                        crystal -= 1;

                        sumPlatinum += 100;

                        platinum += 100;
                    }
                }
            }

            if (crystal > 0)
            {
                while (crystal > 0)
                {
                    byte stack = (byte)Math.Min(100, crystal);

                    await Context.AddCommand(new PlayerInventoryContainerTileCreateItemCommand(Player, 2160, stack) );

                    crystal -= stack;
                }
            }
            else
            {
                foreach (Item item in crystals)
                {
                    if (crystal == 0)
                    {
                        break;
                    }

                    byte stack = (byte)Math.Min( ( (StackableItem)item).Count, -crystal);

                    await Context.AddCommand(new ItemDecrementCommand(item, stack) );

                    crystal += stack;
                }
            }

            if (platinum > 0)
            {
                while (platinum > 0)
                {
                    byte stack = (byte)Math.Min(100, platinum);

                    await Context.AddCommand(new PlayerInventoryContainerTileCreateItemCommand(Player, 2152, stack) );

                    platinum -= stack;
                }
            }
            else
            {
                foreach (Item item in platinums)
                {
                    if (platinum == 0)
                    {
                        break;
                    }

                    byte stack = (byte)Math.Min( ( (StackableItem)item).Count, -platinum);

                    await Context.AddCommand(new ItemDecrementCommand(item, stack) );

                    platinum += stack;
                }
            }

            if (gold > 0)
            {
                while (gold > 0)
                {
                    byte stack = (byte)Math.Min(100, gold);

                    await Context.AddCommand(new PlayerInventoryContainerTileCreateItemCommand(Player, 2148, stack) );

                    gold -= stack;
                }
            }
            else
            {
                foreach (Item item in golds)
                {
                    if (gold == 0)
                    {
                        break;
                    }

                    byte stack = (byte)Math.Min( ( (StackableItem)item).Count, -gold);

                    await Context.AddCommand(new ItemDecrementCommand(item, stack) );

                    gold += stack;
                }
            }

            return true;
        }

        private static int Sum(IContainer parent, List<Item> crystals, List<Item> platinums, List<Item> golds)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += Sum(container, crystals, platinums, golds);
                }

                if (content.Metadata.OpenTibiaId == 2160) // Crystal coin
                {
                    crystals.Add(content);

                    sum += ( (StackableItem)content).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == 2152) // Platinum coin
                {
                    platinums.Add(content);

                    sum += ( (StackableItem)content).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == 2148) // Gold coin
                {
                    golds.Add(content);

                    sum += ( (StackableItem)content).Count * 1;
                }
            }

            return sum;
        }
    }
}