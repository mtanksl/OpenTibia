using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerCountMoneyCommand : CommandResult<int>
    {
        public PlayerCountMoneyCommand(Player player)
        {
            Player = player;
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

                if (content.Metadata.OpenTibiaId == 2160) // Crystal coin
                {
                    sum += ( (StackableItem)content).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == 2152) // Platinum coin
                {
                    sum += ( (StackableItem)content).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == 2148) // Gold coin
                {
                    sum += ( (StackableItem)content).Count * 1;
                }
            }

            return sum;
        }
    }
}