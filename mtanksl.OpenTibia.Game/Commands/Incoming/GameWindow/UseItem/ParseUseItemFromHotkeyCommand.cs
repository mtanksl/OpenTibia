using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemFromHotkeyCommand : ParseUseItemCommand
    {
        public ParseUseItemFromHotkeyCommand(Player player, ushort itemId) : base(player)
        {
            ItemId = itemId;
        }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            int count = Count(Player.Inventory, ItemId);

            if (count > 0)
            {
                Item fromItem = Search(Player.Inventory, ItemId);

                string message;

                if (count == 1)
                {
                    message = "Using the last " + fromItem.Metadata.Name + "...";
                }
                else
                {
                    message = "Using one of " + count + " " + (fromItem.Metadata.Plural ?? fromItem.Metadata.Name) + "...";
                }

                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                return Context.AddCommand(new PlayerUseItemCommand(Player, fromItem, null) );
            }

            return Promise.Break;
        }

        private static int Count(IContainer parent, ushort itemId)
        {
            int count = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    count += Count(container, itemId);
                }

                if (content.Metadata.TibiaId == itemId)
                {
                    if (content is StackableItem stackableItem)
                    {
                        count += stackableItem.Count;
                    }
                    else
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }

        private static Item Search(IContainer parent, ushort itemId)
        {
            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    Item item = Search(container, itemId);

                    if (item != null)
                    {
                        return item;
                    }
                }

                if (content.Metadata.TibiaId == itemId)
                {
                    return content;
                }
            }

            return null;
        }
    }
}