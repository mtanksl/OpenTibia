using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerRemoveMoney : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            int price = (int)conversation.Data["Price"];

            int crystal = 0;

            if (price / 10000 > 0)
            {
                crystal += price / 10000;

                price -= crystal * 10000;
            }

            int platinum = 0;

            if (price / 100 > 0)
            {
                platinum += price / 100;

                price -= platinum * 100;
            }

            int gold = 0;

            if (price / 1 > 0)
            {
                gold += price / 1;

                price -= gold * 1;
            }

            List<Item> crystals = new List<Item>();

            List<Item> platinums = new List<Item>();

            List<Item> golds = new List<Item>();

            Search(player.Inventory, crystals, platinums, golds);

            foreach (Item item in crystals) 
            {
                if (crystal == 0)
                {
                    break;
                }

                int count = ( (StackableItem)item ).Count;

                if (count < crystal)
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)count) );

                    crystal -= count;
                }
                else
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)crystal) );

                    crystal = 0;
                }
            }

            platinum += crystal * 100;

            foreach (Item item in platinums)
            {
                if (platinum == 0)
                {
                    break;
                }

                int count = ( (StackableItem)item ).Count;

                if (count < platinum)
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)count) );

                    platinum -= count;
                }
                else
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)platinum) );

                    platinum = 0;
                }
            }

            gold += platinum * 100;

            foreach (Item item in golds)
            {
                if (gold == 0)
                {
                    break;
                }

                int count = ( (StackableItem)item ).Count;

                if (count < gold)
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)count) );

                    gold -= count;
                }
                else
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)gold) );

                    gold = 0;
                }
            }

            if (gold > 0)
            {
                //TODO: Convert crystal coin to platinum coin and remove
                
                //TODO: Convert platinum coin to gold coin and remove
            }
        }

        private static void Search(IContainer parent, List<Item> crystals, List<Item> platinums, List<Item> golds)
        {
            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    Search(container, crystals, platinums, golds);
                }

                if (content.Metadata.OpenTibiaId == 2160) // Crystal coin
                {
                    crystals.Add(content);
                }
                else if (content.Metadata.OpenTibiaId == 2152) // Platinum coin
                {
                    platinums.Add(content);
                }
                else if (content.Metadata.OpenTibiaId == 2148) // Gold coin
                {
                    golds.Add(content);
                }
            }
        }
    }
}