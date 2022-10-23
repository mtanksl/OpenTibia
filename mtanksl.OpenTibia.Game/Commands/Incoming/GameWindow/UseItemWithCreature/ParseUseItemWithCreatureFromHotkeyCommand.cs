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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                foreach (var pair in fromInventory.GetIndexedContents() )
                {
                    Item fromItem = (Item)pair.Value;

                    if (fromItem.Metadata.TibiaId == ItemId)
                    {
                        Creature toCreature = context.Server.GameObjects.GetCreature(ToCreatureId);

                        if (toCreature != null)
                        {
                            if ( IsUseable(context, fromItem) )
                            {
                                context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) ).Then(ctx =>
                                {
                                    resolve(ctx);
                                } );
                            }
                        }

                        break;
                    }
                }
            } );            
        }
    }
}