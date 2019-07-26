using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class RotateItemFromInventoryCommand : Command
    {
        public RotateItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId)
        {
            Player = player;

            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Rotatable) )
                {
                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                }
                else
                {
                    //Act

                    ItemUseScript script;

                    if ( !server.ItemRotateScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, server, context) )
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