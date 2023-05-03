using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerAddMoney : TopicCallback
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

            while (crystal > 0)
            {
                if (crystal > 100)
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2160, 100) );

                    crystal -= 100;
                }
                else
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2160, (byte)crystal) );

                    crystal = 0;
                }
            }

            while (platinum > 0)
            {
                if (platinum > 100)
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2152, 100) );

                    platinum -= 100;
                }
                else
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2152, (byte)platinum) );

                    platinum = 0;
                }
            }

            while (gold > 0)
            {
                if (gold > 100)
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2148, 100) );

                    gold -= 100;
                }
                else
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2148, (byte)gold) );

                    gold = 0;
                }
            }
        }
    }
}