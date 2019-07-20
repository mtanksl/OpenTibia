using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromInventoryCommand : LookCommand
    {
        public LookFromInventoryCommand(Player player, byte fromSlot, ushort itemId)
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
                //Act

                Look(Player, fromItem, server, context);
            }

            base.Execute(server, context);
        }
    }
}