using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromInventoryCommand : LookCommand
    {
        public LookFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item item = fromInventory.GetContent(FromSlot) as Item;

            if (item != null && item.Metadata.TibiaId == ItemId)
            {
                //Act

                LookItem(item, server, context);
            }
        }
    }
}