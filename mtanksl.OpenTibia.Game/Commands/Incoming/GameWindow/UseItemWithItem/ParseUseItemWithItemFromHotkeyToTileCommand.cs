using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromHotkeyToTileCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromHotkeyToTileCommand(Player player, ushort fromTibiaId, Position toPosition, byte toIndex, ushort toTibiaId) : base(player)
        {
            FromTibiaId = fromTibiaId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToTibiaId = toTibiaId;
        }

        public ushort FromTibiaId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            int sum = Sum(Player.Inventory, FromTibiaId);

            if (sum > 0)
            {
                Item fromItem = Search(Player.Inventory, FromTibiaId);

                string message;

                if (sum == 1)
                {
                    message = "Using the last " + fromItem.Metadata.Name + "...";
                }
                else
                {
                    message = "Using one of " + sum + " " + (fromItem.Metadata.Plural ?? fromItem.Metadata.Name) + "...";
                }

                Tile toTile = Context.Server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    switch (Player.Client.GetContent(toTile, ToIndex) )
                    {
                        case Item toItem:

                            if (toItem.Metadata.TibiaId == ToTibiaId)
                            {
                                if ( IsUseable(fromItem) )
                                {
                                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                                    return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                                }
                            }

                            break;

                        case Creature toCreature:

                            if (ToTibiaId == 99)
                            {
                                if ( IsUseable(fromItem) )
                                {
                                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                                    return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                                }
                            }

                            break;
                    }
                }
            }

            return Promise.Break;
        }

        private static int Sum(IContainer parent, ushort tibiaId)
        {
            int sum = 0;

            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    sum += Sum(container, tibiaId);
                }

                if (content.Metadata.TibiaId == tibiaId)
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

        private static Item Search(IContainer parent, ushort tibiaId)
        {
            foreach (Item content in parent.GetContents() )
            {
                if (content is Container container)
                {
                    Item item = Search(container, tibiaId);

                    if (item != null)
                    {
                        return item;
                    }
                }

                if (content.Metadata.TibiaId == tibiaId)
                {
                    return content;
                }
            }

            return null;
        }
    }
}