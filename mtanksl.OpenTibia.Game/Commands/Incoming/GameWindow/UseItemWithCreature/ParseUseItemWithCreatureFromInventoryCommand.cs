using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithCreatureFromInventoryCommand : ParseUseItemWithCreatureCommand
    {
        public ParseUseItemWithCreatureFromInventoryCommand(Player player, byte fromSlot, ushort itemId, uint toCreatureId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
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
                Creature toCreature = Context.Server.GameObjects.GetCreature(ToCreatureId);

                if (toCreature != null)
                {
                    if ( IsUseable(fromItem) )
                    {
                        return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}