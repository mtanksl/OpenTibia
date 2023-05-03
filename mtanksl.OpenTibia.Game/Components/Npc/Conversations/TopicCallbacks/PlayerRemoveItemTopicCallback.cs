using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerRemoveItem : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            ushort openTibiaId = (ushort)(int)conversation.Data["Type"];

            int amount = (int)conversation.Data["Amount"];

            List<Item> items = new List<Item>();

            Search(player.Inventory, openTibiaId, items);

            foreach (Item item in items)
            {
                if (amount == 0)
                {
                    break;
                }

                if (item is StackableItem stackableItem)
                {
                    int count = stackableItem.Count;

                    if (count < amount)
                    {
                        await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)count) );

                        amount -= count;
                    }
                    else
                    {
                        await Context.Current.AddCommand(new ItemDecrementCommand(item, (byte)amount) );

                        amount = 0;
                    }
                }
                else
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, 1) );

                    amount -= 1;
                }
            }
        }

        private static void Search(IContainer parent, ushort openTibiaId, List<Item> items)
        {
            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    Search(container, openTibiaId, items);
                }

                if (content.Metadata.OpenTibiaId == openTibiaId)
                {
                    items.Add(content);
                }
            }
        }
    }
}