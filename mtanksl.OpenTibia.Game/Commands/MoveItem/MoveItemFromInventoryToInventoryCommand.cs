using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToInventoryCommand : Command
    {
        public MoveItemFromInventoryToInventoryCommand(Player player, byte fromSlot, byte toSlot)
        {
            Player = player;

            FromSlot = fromSlot;

            ToSlot = toSlot;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public byte ToSlot { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = (Item)fromInventory.GetContent(FromSlot);

            Item toItem = (Item)fromInventory.GetContent(ToSlot);

            //Act
            
            if (toItem == null)
            {
                fromInventory.RemoveContent(fromItem);

                fromInventory.AddContent(ToSlot, fromItem);

                //Notify

                context.Write(Player.Client.Connection, new SlotRemoveOutgoingPacket( (Slot)FromSlot ),

                                                        new SlotAddOutgoingPacket( (Slot)ToSlot, fromItem ) );
            }
            else
            {
                //Notify

                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
            }
        }
    }
}