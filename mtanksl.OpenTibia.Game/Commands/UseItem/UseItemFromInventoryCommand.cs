﻿using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromInventoryCommand : UseItemCommand
    {
        public UseItemFromInventoryCommand(Player player, byte fromSlot, ushort itemId) : base(player)
        {
            FromSlot = fromSlot;

            ItemId = itemId;
        }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                //Act

                UseItem(fromItem, server, context);
            }
        }
    }
}