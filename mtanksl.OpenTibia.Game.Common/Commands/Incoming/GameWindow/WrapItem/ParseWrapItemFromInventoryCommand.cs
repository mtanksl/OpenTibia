﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseWrapItemFromInventoryCommand : ParseWrapItemCommand
    {
        public ParseWrapItemFromInventoryCommand(Player player, byte fromSlot, ushort tibiaId) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
            {
                if ( IsWrapable(fromItem) )
                {
                    return Context.AddCommand(new PlayerWrapItemCommand(Player, fromItem) );
                }
            }

            return Promise.Break;
        }
    }
}