using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromInventoryToInventoryCommand : Command
    {
        public UseItemWithItemFromInventoryToInventoryCommand(Player player, byte fromSlot, ushort fromItemId, byte toSlot, ushort toItemId)
        {
            Player = player;

            FromSlot = fromSlot;

            FromItemId = fromItemId;

            ToSlot = toSlot;

            ToItemId = toItemId;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToSlot { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
            {
                Inventory toInventory = Player.Inventory;

                Item toItem = toInventory.GetContent(ToSlot) as Item;

                if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                {
                    if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                    {
                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                    }
                    else
                    {
                        //Act

                        ItemUseWithItemScript script;

                        if ( !server.ItemUseWithItemScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, toItem, server, context) )
                        {
                            //Notify

                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                        }
                        else
                        {
                            base.Execute(server, context);
                        }
                    }
                }
            }
        }
    }
}