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

        public override void Execute(Context context)
        {
            Inventory fromInventory = Player.Inventory;

            Item item = fromInventory.GetContent(FromSlot) as Item;

            if (item != null && item.Metadata.TibiaId == ItemId)
            {
                LookAtItem(context, item);
            }
        }
    }
}