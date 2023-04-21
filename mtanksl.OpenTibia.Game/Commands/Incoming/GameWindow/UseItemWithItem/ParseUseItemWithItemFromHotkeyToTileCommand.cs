using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromHotkeyToTileCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromHotkeyToTileCommand(Player player, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) : base(player)
        {
            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToItemId = toItemId;
        }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

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

                Tile toTile = Context.Server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    switch (toTile.GetContent(ToIndex) )
                    {
                        case Item toItem:

                            if (toItem.Metadata.TibiaId == ToItemId)
                            {
                                if ( IsUseable(fromItem) )
                                {
                                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                                    return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                                }
                            }

                            break;

                        case Creature toCreature:

                            if (ToItemId == 99)
                            {
                                if ( IsUseable(fromItem) )
                                {
                                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                                    return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                                }
                            }

                            break;
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