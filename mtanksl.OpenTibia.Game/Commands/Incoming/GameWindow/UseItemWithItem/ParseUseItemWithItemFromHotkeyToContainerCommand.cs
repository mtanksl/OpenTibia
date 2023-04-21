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
            int count = Count(Player.Inventory, FromItemId);

            if (count > 0)
            {
                Item fromItem = Search(Player.Inventory, FromItemId);

                string message;

                if (count == 1)
                {
                    message = "Using the last " + fromItem.Metadata.Name + "...";
                }
                else
                {
                    message = "Using one of " + count + " " + (fromItem.Metadata.Plural ?? fromItem.Metadata.Name) + "...";
                }

                Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

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