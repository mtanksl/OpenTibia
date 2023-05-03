using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerHasMoney : TopicCondition
    {
        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            int price = (int)conversation.Data["Price"];

            if (Sum(player.Inventory) >= price)
            {
                return true;
            }

            return false;
        }

        private static int Sum(IContainer parent)
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
                    sum += ( (StackableItem)content ).Count * 10000;
                }
                else if (content.Metadata.OpenTibiaId == 2152) // Platinum coin
                {
                    sum += ( (StackableItem)content ).Count * 100;
                }
                else if (content.Metadata.OpenTibiaId == 2148) // Gold coin
                {
                    sum += ( (StackableItem)content ).Count * 1;
                }                
            }

            return sum;
        }
    }
}