using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerHasItem : TopicCondition
    {
        public override bool Handle(Conversation conversation, Npc npc, Player player, string message)
        {
            ushort openTibiaId = (ushort)(int)conversation.Data["Type"];

            //byte count = (byte)(int)conversation.Data["Data"];

            int amount = (int)conversation.Data["Amount"];

            if (Sum(player.Inventory, openTibiaId) >= amount)
            {
                return true;
            }

            return false;
        }

        private static int Sum(IContainer parent, ushort openTibiaId)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += Sum(container, openTibiaId);
                }

                if (content.Metadata.OpenTibiaId == openTibiaId)
                {
                    if (content is StackableItem stackableItem)
                    {
                        sum += stackableItem.Count;
                    }
                    else
                    {
                        sum += 1;
                    }
                }
            }

            return sum;
        }
    }
}