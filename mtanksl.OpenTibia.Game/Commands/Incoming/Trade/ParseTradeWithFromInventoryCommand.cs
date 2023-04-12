using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTradeWithFromInventoryCommand : ParseTradeWithCommand
    {
        public ParseTradeWithFromInventoryCommand(Player player, byte fromSlot, ushort itemId, uint creatureId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToCreatureId = creatureId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Player toPlayer = Context.Server.GameObjects.GetPlayer(ToCreatureId);

                if (toPlayer != null && toPlayer != Player)
                {
                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}