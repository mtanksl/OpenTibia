using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToInventoryCommand : Command
    {
        private Server server;

        public MoveItemFromInventoryToInventoryCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public byte ToSlot { get; set; }
        
        public override void Execute(Context context)
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

                context.Response.Write(Player.Client.Connection, new SlotRemove( (Slot)FromSlot) )

                    .Write(Player.Client.Connection, new SlotAdd( (Slot)ToSlot, fromItem) );
            }
            else
            {
                //Notify

                context.Response.Write(Player.Client.Connection, new ShowWindowText(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
            }
        }
    }
}