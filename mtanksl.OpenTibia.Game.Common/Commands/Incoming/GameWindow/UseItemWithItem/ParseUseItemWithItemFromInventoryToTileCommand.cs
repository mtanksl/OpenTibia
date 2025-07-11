﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromInventoryToTileCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromInventoryToTileCommand(Player player, byte fromSlot, ushort fromTibiaId, Position toPosition, byte toIndex, ushort toTibiaId) : base(player)
        {
            FromSlot = fromSlot;

            FromTibiaId = fromTibiaId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToTibiaId = toTibiaId;
        }

        public byte FromSlot { get; set; }

        public ushort FromTibiaId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
            {
                Tile toTile = Context.Server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    if (Player.Tile.Position.Z == toTile.Position.Z)
                    {
                        if (Player.Tile.Position.CanHearSay(toTile.Position) )
                        {
                            switch (Player.Client.GetContent(toTile, ToIndex) )
                            {
                                case Item toItem:

                                    if (toItem.Metadata.TibiaId == ToTibiaId)
                                    {
                                        if ( IsUseable(fromItem) )
                                        {
                                            return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                                        }
                                    }

                                    break;

                                case Creature toCreature:

                                    if (ToTibiaId == 99)
                                    {
                                        if ( IsUseable(fromItem) )
                                        {
                                            return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (Player.Tile.Position.Z > toTile.Position.Z)
                        {
                            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.FirstGoUpstairs) );
                        }
                        else
                        {
                            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.FirstGoDownstairs) );
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}