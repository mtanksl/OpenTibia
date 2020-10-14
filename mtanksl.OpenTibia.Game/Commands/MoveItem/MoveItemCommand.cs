using OpenTibia.Common.Objects;
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

        protected bool IsMoveable(Item fromItem, byte count, Context context)
        {
            if ( fromItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) )
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotMoveThisObject) );

                return false;
            }

            if (fromItem is StackableItem stackableItem)
            {
                if (count < 1 || count > stackableItem.Count)
                {
                    return false;
                }
            }
            else
            {
                if (count != 1)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsPickupable(Item fromItem, Context context)
        {
            if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Pickupable) )
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTakeThisObject) );

                return false;
            }

            return true;
        }

        protected bool CanThrow(Tile fromTile, Tile toTile, Context context)
        {
            if ( !context.Server.Pathfinding.CanThrow(fromTile.Position, toTile.Position) )
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                return false;
            }

            return true;
        }

        protected bool IsPossible(Item fromItem, Container toContainer, Context context)
        {
            if ( toContainer.IsChild(fromItem) )
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );

                return false;
            }

            return true;
        }
        
        protected bool IsEnoughtSpace(Item fromItem, Container toContainer, Context context)
        {
            if (toContainer.Count == toContainer.Metadata.Capacity)
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtSpace) );

                return false;
            }

            return true;
        }

        protected void MoveItem(Item fromItem, IContainer toContainer, byte toIndex, byte count, Context context)
        {
            Command command = context.TransformCommand(new PlayerMoveItemCommand(Player, fromItem, toContainer, toIndex, count) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}