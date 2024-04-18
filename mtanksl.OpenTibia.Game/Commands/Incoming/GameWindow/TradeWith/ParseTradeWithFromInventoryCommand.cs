using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseTradeWithFromInventoryCommand : ParseTradeWithCommand
    {
        public ParseTradeWithFromInventoryCommand(Player player, byte fromSlot, ushort tibiaId, uint creatureId) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;

            ToCreatureId = creatureId;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
            {
                Player toPlayer = Context.Server.GameObjects.GetPlayer(ToCreatureId);

                if (toPlayer != null && toPlayer != Player)
                {
                    if ( IsPickupable(fromItem) )
                    {
                        return Context.AddCommand(new PlayerTradeWithCommand(Player, fromItem, toPlayer) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}