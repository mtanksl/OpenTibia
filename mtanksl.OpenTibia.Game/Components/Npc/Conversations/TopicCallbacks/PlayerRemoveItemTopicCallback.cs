using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerRemoveItem : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            ushort openTibiaId = (ushort)(int)conversation.Data["Type"];

            //byte count = (byte)(int)conversation.Data["Data"];

            int amount = (int)conversation.Data["Amount"];

            List<Item> items = new List<Item>();

            Sum(player.Inventory, openTibiaId, items);

            foreach (Item item in items)
            {
                if (amount == 0)
                {
                    break;
                }

                if (item is StackableItem stackableItem)
                {
                    byte count = (byte)Math.Min(stackableItem.Count, amount);

                    await Context.Current.AddCommand(new ItemDecrementCommand(item, count) );

                    amount -= count;
                }
                else
                {
                    await Context.Current.AddCommand(new ItemDecrementCommand(item, 1) );

                    amount -= 1;
                }
            }
        }

        private static int Sum(IContainer parent, ushort openTibiaId, List<Item> items)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += Sum(container, openTibiaId, items);
                }

                if (content.Metadata.OpenTibiaId == openTibiaId)
                {
                    items.Add(content);

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