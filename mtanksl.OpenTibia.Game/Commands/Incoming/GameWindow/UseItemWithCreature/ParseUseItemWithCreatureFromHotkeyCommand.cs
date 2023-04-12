using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithCreatureFromHotkeyCommand : ParseUseItemWithCreatureCommand
    {
        public ParseUseItemWithCreatureFromHotkeyCommand(Player player, ushort itemId, uint toCreatureId) : base(player)
        {
            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            foreach (var pair in fromInventory.GetIndexedContents() )
            {
                Item fromItem = (Item)pair.Value;

                if (fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = Context.Server.GameObjects.GetCreature(ToCreatureId);

                    if (toCreature != null)
                    {
                        if ( IsUseable(Context, fromItem) )
                        {
                            return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                        }
                    }

                    break;
                }
            }

            return Promise.Break;
        }
    }
}