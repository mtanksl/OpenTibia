using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseRotateItemFromInventoryCommand : ParseRotateItemCommand
    {
        public ParseRotateItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    if ( IsRotatable(context, fromItem) )
                    {
                        context.AddCommand(new PlayerRotateItemCommand(Player, fromItem) ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );
                    }
                }
            } );
        }
    }
}