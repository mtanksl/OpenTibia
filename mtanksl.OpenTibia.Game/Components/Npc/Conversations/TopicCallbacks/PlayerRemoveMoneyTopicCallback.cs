using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerRemoveMoney : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            int price = (int)conversation.Data["Price"];

            int crystal = 0;

            int platinum = 0;

            int gold = 0;

            List<Item> crystals = new List<Item>();

            List<Item> platinums = new List<Item>();

            List<Item> golds = new List<Item>();

            Sum(player.Inventory, crystals, platinums, golds);

            int sumCrystal = crystals.Sum(i => ( (StackableItem)i ).Count);

            int sumPlatinum = platinums.Sum(i => ( (StackableItem)i ).Count);

            int sumGold = golds.Sum(i => ( (StackableItem)i ).Count);

            while (price > 0)
            {
                var n = Math.Min(price / 10000, sumCrystal);

                if (n > 0)
                {
                    price -= n * 10000;

                    sumCrystal -= n;

                    crystal -= n;
                }

                n = Math.Min(price / 100, sumPlatinum);

                if (n > 0)
                {
                    price -= n * 100;

                    sumPlatinum -= n;

                    platinum -= n;
                }

                n = Math.Min(price / 1, sumGold);

                if (n > 0)
                {
                    price -= n * 1;

                    sumGold -= n;

                    gold -= n;
                }

                if (price > 0)
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
                    byte count = (byte)Math.Min(100, crystal);

                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2160, count) );

                    crystal -= count;
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

                    byte count = (byte)Math.Min( ( (StackableItem)item ).Count, -crystal);

                    await Context.Current.AddCommand(new ItemDecrementCommand(item, count) );

                    crystal += count;
                }
            }

            if (platinum > 0)
            {
                while (platinum > 0)
                {
                    byte count = (byte)Math.Min(100, platinum);

                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2152, count) );

                    platinum -= count;
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

                    byte count = (byte)Math.Min( ( (StackableItem)item).Count, -platinum);

                    await Context.Current.AddCommand(new ItemDecrementCommand(item, count) );

                    platinum += count;
                }
            }

            if (gold > 0)
            {
                while (gold > 0)
                {
                    byte count = (byte)Math.Min(100, gold);

                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2148, count) );

                    gold -= count;
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

                    byte count = (byte)Math.Min( ( (StackableItem)item ).Count, -gold);

                    await Context.Current.AddCommand(new ItemDecrementCommand(item, count) );

                    gold += count;
                }
            }
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

                    sum += ( (StackableItem)content ).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == 2152) // Platinum coin
                {
                    platinums.Add(content);

                    sum += ( (StackableItem)content ).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == 2148) // Gold coin
                {
                    golds.Add(content);

                    sum += ( (StackableItem)content ).Count * 1;
                }
            }

            return sum;
        }
    }
}