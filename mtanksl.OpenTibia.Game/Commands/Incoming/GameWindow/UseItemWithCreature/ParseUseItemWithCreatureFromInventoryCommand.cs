using OpenTibia.Common.Objects;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Inventory fromInventory = Player.Inventory;

                Item fromItem = fromInventory.GetContent(FromSlot) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = context.Server.GameObjects.GetGameObject<Creature>(ToCreatureId);

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
                }
            } );            
        }
    }
}