using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class PlayerCountMoneyCommand : CommandResult<int>
    {
        private readonly ushort goldCoin;
        private readonly ushort platinumCoin;
        private readonly ushort crystalCoin;

        public PlayerCountMoneyCommand(Player player)
        {
            Player = player;

            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            crystalCoin = Context.Server.Values.GetUInt16("values.items.crystalCoin");
        }

        public Player Player { get; set; }

        public override PromiseResult<int> Execute()
        {
            int sum = Sum(Player.Inventory);

            return Promise.FromResult(sum);            
        }

        private int Sum(IContainer parent)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += Sum(container);
                }

                if (content.Metadata.OpenTibiaId == crystalCoin)
                {
                    sum += ( (StackableItem)content).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == platinumCoin)
                {
                    sum += ( (StackableItem)content).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == goldCoin)
                {
                    sum += ( (StackableItem)content).Count * 1;
                }
            }

            return sum;
        }
    }
}