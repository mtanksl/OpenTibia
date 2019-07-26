using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromInventoryCommand : Command
    {
        public UseItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId)
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
                Container container = fromItem as Container;

                if (container != null)
                {
                    Command command = new OpenOrCloseContainerCommand(Player, container);

                    command.Completed += (s, e) =>
                    {
                        //Act

                        base.Execute(server, context);
                    };

                    command.Execute(server, context);
                }
                else
                {
                    ItemUseScript script;

                    if ( !server.ItemUseScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, server, context) )
                    {
                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
                    }
                    else
                    {
                        //Act

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}