﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class MoveItemCommand : Command
    {
        public MoveItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsNextTo(Tile fromTile, Server server, Context context)
        {
            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
            {
                WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                walkToUnknownPathCommand.Completed += (s, e) =>
                {
                    server.QueueForExecution(Constants.PlayerActionSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
                };

                walkToUnknownPathCommand.Execute(server, context);

                return false;
            }

            return true;
        }

        protected bool IsMoveable(Item fromItem, Server server, Context context)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            return true;
        }

        protected bool IsPickupable(Item fromItem, Server server, Context context)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                return false;
            }

            return true;
        }

        protected bool CanThrow(Tile fromTile, Tile toTile, Server server, Context context)
        {
            if ( !server.Pathfinding.CanThrow(fromTile.Position, toTile.Position) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                return false;
            }

            return true;
        }

        protected bool IsPossible(Item fromItem, Container toContainer, Server server, Context context)
        {
            if ( toContainer.IsChild(fromItem) )
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );

                return false;
            }

            return true;
        }
        
        protected bool IsEnoughtSpace(Item fromItem, Container toContainer, Server server, Context context)
        {
            if (toContainer.Count == toContainer.Metadata.Capacity)
            {
                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtSpace) );

                return false;
            }

            return true;
        }

        protected void MoveItem(Item fromItem, IContainer toContainer, byte toIndex, byte count, Server server, Context context)
        {
            new ItemMoveCommand(Player, fromItem, toContainer, toIndex, count).Execute(server, context);

            base.Execute(server, context);
        }
    }
}