using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemFromHotkeyCommand : ParseUseItemCommand
    {
        public ParseUseItemFromHotkeyCommand(Player player, ushort itemId) : base(player)
        {
            ItemId = itemId;
        }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {             
                Inventory fromInventory = Player.Inventory;

                foreach (var pair in fromInventory.GetIndexedContents() )
                {
                    Item fromItem = (Item)pair.Value;

                    if (fromItem.Metadata.TibiaId == ItemId)
                    {
                        context.AddCommand(new PlayerUseItemCommand(Player, fromItem, null) ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );

                        break;
                    }
                }
            } ); 
        }
    }
}