using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromHotkeyToContainerCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromHotkeyToContainerCommand(Player player, ushort fromItemId, byte toContainerId, byte toContainerIndex, ushort toItemId) :base(player)
        {
            FromItemId = fromItemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToItemId = toItemId;
        }

        public ushort FromItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override Promise Execute()
        {
            int sum = Sum(Player.Inventory, FromItemId);

            if (sum > 0)
            {
                Item fromItem = Search(Player.Inventory, FromItemId);

                string message;

                if (sum == 1)
                {
                    message = "Using the last " + fromItem.Metadata.Name + "...";
                }
                else
                {
                    message = "Using one of " + sum + " " + (fromItem.Metadata.Plural ?? fromItem.Metadata.Name) + "...";
                }

                Container toContainer = Player.Client.Containers.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                    if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                    {
                        if ( IsUseable(fromItem) )
                        {
                            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                            return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                        }
                    }
                }
            }

            return Promise.Break;
        }

        private static int Sum(IContainer parent, ushort itemId)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += Sum(container, itemId);
                }

                if (content.Metadata.TibiaId == itemId)
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