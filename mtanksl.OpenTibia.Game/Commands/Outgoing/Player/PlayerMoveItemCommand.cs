using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveItemCommand : Command
    {
        public PlayerMoveItemCommand(Player player, Item item, IContainer toContainer, byte toIndex, byte count)
        {
            Player = player;

            Item = item;

            ToContainer = toContainer;

            ToIndex = toIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public IContainer ToContainer { get; set; }

        public byte ToIndex { get; set; }

        public byte Count { get; set; }

        protected bool CanThrow(Context context, Tile fromTile, Tile toTile)
        {
            if (toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                return false;
            }

            if ( !context.Server.Pathfinding.CanThrow(fromTile.Position, toTile.Position) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotThrowThere) );

                return false;
            }

            return true;
        }

        protected bool IsEmpty(Context context, Item fromItem, Inventory toInventory, byte toSlot)
        {
            if (toInventory.GetContent(toSlot) != null)
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                return false;
            }

            return true;
        }

        protected bool IsEnoughtSpace(Context context, Item fromItem, Container toContainer)
        {
            if (toContainer.Count == toContainer.Metadata.Capacity)
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtSpace) );

                return false;
            }

            return true;
        }

        public override void Execute(Context context)
        {
            switch (ToContainer)
            {
                case Tile toTile:
                {
                    Command command = new ItemUpdateParentToTileCommand(Item, toTile);

                    ICommandHandler commandHandler;

                    if (context.Server.CommandHandlers.TryGet(context, command, out commandHandler) )
                    {
                        new Promise(resolve =>
                        {
                            commandHandler.ContinueWith = resolve;

                            commandHandler.Handle(context, command);

                        } ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                    }
                    else
                    {
                        if (CanThrow(context, Item.Parent is Tile fromTile ? fromTile : Player.Tile, toTile) )
                        {
                            new Promise(resolve =>
                            {
                                command.ContinueWith = resolve;

                                command.Execute(context);

                            } ).Then(ctx =>
                            {
                                OnComplete(ctx);
                            } );
                        }
                    }
                }

                break;

                case Inventory toInventory:
                {
                    Command command = new ItemUpdateParentToInventoryCommand(Item, toInventory, ToIndex);

                    ICommandHandler commandHandler;

                    if (context.Server.CommandHandlers.TryGet(context, command, out commandHandler) )
                    {
                        new Promise(resolve =>
                        {
                            commandHandler.ContinueWith = resolve;

                            commandHandler.Handle(context, command);

                        } ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                    }
                    else
                    {
                        if (IsEmpty(context, Item, toInventory, ToIndex) )
                        {
                            new Promise(resolve =>
                            {
                                command.ContinueWith = resolve;

                                command.Execute(context);

                            } ).Then(ctx =>
                            {
                                OnComplete(ctx);
                            } );
                        }
                    }
                }

                break;

                case Container toContainer:
                {
                    Command command = new ItemUpdateParentToContainerCommand(Item, toContainer);

                    ICommandHandler commandHandler;

                    if (context.Server.CommandHandlers.TryGet(context, command, out commandHandler) )
                    {
                        new Promise(resolve =>
                        {
                            commandHandler.ContinueWith = resolve;

                            commandHandler.Handle(context, command);

                        } ).Then(ctx =>
                        {
                            OnComplete(ctx);
                        } );
                    }
                    else
                    {
                        if (IsEnoughtSpace(context, Item, toContainer) )
                        {
                            new Promise(resolve =>
                            {
                                command.ContinueWith = resolve;

                                command.Execute(context);

                            } ).Then(ctx =>
                            {
                                OnComplete(ctx);
                            } );
                        }
                    }
                }

                break;
            }
        }
    }
}