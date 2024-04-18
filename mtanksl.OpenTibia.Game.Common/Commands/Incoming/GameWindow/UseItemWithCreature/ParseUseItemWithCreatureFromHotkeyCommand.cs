using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithCreatureFromHotkeyCommand : ParseUseItemWithCreatureCommand
    {
        public ParseUseItemWithCreatureFromHotkeyCommand(Player player, ushort tibiaId, uint toCreatureId) : base(player)
        {
            TibiaId = tibiaId;

            ToCreatureId = toCreatureId;
        }

        public ushort TibiaId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            int sum = Sum(Player.Inventory, TibiaId);

            if (sum > 0)
            {
                Item fromItem = Search(Player.Inventory, TibiaId);

                string message;

                if (sum == 1)
                {
                    message = "Using the last " + fromItem.Metadata.Name + "...";
                }
                else
                {
                    message = "Using one of " + sum + " " + (fromItem.Metadata.Plural ?? fromItem.Metadata.Name) + "...";
                }

                Creature toCreature = Context.Server.GameObjects.GetCreature(ToCreatureId);

                if (toCreature != null)
                {
                    if ( IsUseable(fromItem) )
                    {
                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );

                        return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
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