using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        protected bool IsEnoughtRoom(Context context, Tile fromTile, Tile toTile)
        {
            if (toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThereIsNotEnoughtRoom) );

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
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCannotPutMoreObjectsInThisContainer) );

                return false;
            }

            return true;
        }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                switch (ToContainer)
                {
                    case Tile toTile:

                        if (IsEnoughtRoom(context, Item.Parent is Tile fromTile ? fromTile : Player.Tile, toTile) )
                        {
                            context.AddCommand(new ItemUpdateParentToTileCommand(Item, toTile) ).Then(ctx =>
                            {
                                resolve(ctx);
                            } );
                        }

                        break;

                    case Inventory toInventory:

                        if (IsEmpty(context, Item, toInventory, ToIndex) )
                        {
                            context.AddCommand(new ItemUpdateParentToInventoryCommand(Item, toInventory, ToIndex) ).Then(ctx =>
                            {
                                resolve(ctx);
                            } );
                        }

                        break;

                    case Container toContainer:

                        if (IsEnoughtSpace(context, Item, toContainer) )
                        {
                            context.AddCommand(new ItemUpdateParentToContainerCommand(Item, toContainer) ).Then(ctx =>
                            {
                                resolve(ctx);
                            } );
                        }
                        
                        break;
                }
            } );
        }
    }
}