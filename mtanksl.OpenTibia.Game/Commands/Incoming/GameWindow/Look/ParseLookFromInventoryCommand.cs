using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromInventoryCommand : ParseLookCommand
    {
        public ParseLookFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                Item item = fromInventory.GetContent(FromSlot) as Item;

                if (item != null && item.Metadata.TibiaId == ItemId)
                {
                    context.AddCommand(new PlayerLookItemCommand(Player, item) ).Then(ctx =>
                    {
                        resolve(ctx);
                    } );
                }
            } );
        }
    }
}