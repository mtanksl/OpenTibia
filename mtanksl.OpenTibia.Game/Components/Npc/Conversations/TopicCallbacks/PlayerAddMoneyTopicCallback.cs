using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerAddMoney : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            int price = (int)conversation.Data["Price"];

            int crystal = 0;

            int platinum = 0;

            int gold = 0;

            int n = price / 10000;

            if (n > 0)
            {
                price -= n * 10000;

                crystal += n;
            }

            n = price / 100;

            if (n > 0)
            {
                price -= n * 100;

                platinum += n;
            }

            n = price / 1;

            if (n > 0)
            {
                price -= n * 1;

                gold += n;
            }

            while (crystal > 0)
            {
                byte count = (byte)Math.Min(100, crystal);

                await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2160, count) );

                crystal -= count;
            }

            while (platinum > 0)
            {
                byte count = (byte)Math.Min(100, platinum);

                await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2152, count) );

                platinum -= count;
            }

            while (gold > 0)
            {
                byte count = (byte)Math.Min(100, gold);

                await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, 2148, count) );

                gold -= count;
            }
        }
    }
}