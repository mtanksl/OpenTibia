﻿using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TradeWithFromInventoryCommand : TradeWithCommand
    {
        public TradeWithFromInventoryCommand(Player player, byte fromSlot, ushort itemId, uint creatureId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;

            ToCreatureId = creatureId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Context context)
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Player toPlayer = context.Server.GameObjects.GetGameObject<Player>(ToCreatureId);

                if (toPlayer != null && toPlayer != Player)
                {
                    TradeWith(context, fromItem, toPlayer);
                }
            }            
        }
    }
}