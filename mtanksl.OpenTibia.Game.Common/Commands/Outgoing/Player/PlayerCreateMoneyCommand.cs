using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerCreateMoneyCommand : Command
    {
        private readonly ushort goldCoin;
        private readonly ushort platinumCoin;
        private readonly ushort crystalCoin;

        public PlayerCreateMoneyCommand(Player player, int price)
        {
            Player = player;

            Price = price;

            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            crystalCoin = Context.Server.Values.GetUInt16("values.items.crystalCoin");
        }

        public Player Player { get; set; }

        public int Price { get; set; }

        public override async Promise Execute()
        {
            int crystal = 0;

            int platinum = 0;

            int gold = 0;

            int n = Price / 10000;

            if (n > 0)
            {
                Price -= n * 10000;

                crystal += n;
            }

            n = Price / 100;

            if (n > 0)
            {
                Price -= n * 100;

                platinum += n;
            }

            n = Price / 1;

            if (n > 0)
            {
                Price -= n * 1;

                gold += n;
            }

            while (crystal > 0)
            {
                byte stack = (byte)Math.Min(100, crystal);

                await Context.AddCommand(new PlayerCreateItemCommand(Player, crystalCoin, stack) );

                crystal -= stack;
            }

            while (platinum > 0)
            {
                byte stack = (byte)Math.Min(100, platinum);

                await Context.AddCommand(new PlayerCreateItemCommand(Player, platinumCoin, stack) );

                platinum -= stack;
            }

            while (gold > 0)
            {
                byte stack = (byte)Math.Min(100, gold);

                await Context.AddCommand(new PlayerCreateItemCommand(Player, goldCoin, stack) );

                gold -= stack;
            }
        }
    }
}