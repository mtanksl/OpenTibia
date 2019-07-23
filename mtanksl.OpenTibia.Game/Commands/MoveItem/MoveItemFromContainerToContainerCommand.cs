using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToContainerCommand : MoveItemCommand
    {
        public MoveItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        if ( toContainer.IsChildOfParent(fromItem) )
                        {
                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisIsImpossible) );
                        }
                        else
                        {
                            //Act

                            RemoveItem(fromContainer, FromContainerIndex, server, context);

                            AddItem(toContainer, fromItem, server, context);

                            Container container = fromItem as Container;

                            if (container != null)
                            {
                                switch (fromContainer.GetParent() )
                                {
                                    case Tile fromTile:

                                        switch (toContainer.GetParent() )
                                        {
                                            case Tile toTile:

                                                CloseContainer(fromTile, toTile, container, server, context);

                                                break;

                                            case Inventory toInventory:

                                                CloseContainer(fromTile, toInventory, container, server, context);

                                                break;
                                        }

                                        break;

                                    case Inventory fromInventory:

                                        switch (toContainer.GetParent() )
                                        {
                                            case Tile toTile:

                                                CloseContainer(fromInventory, toTile, container, server, context);

                                                break;

                                            case Inventory toInventory:

                                                CloseContainer(fromInventory, toInventory, container, server, context);

                                                break;
                                        }
                                            
                                        break;
                                }
                            }

                            base.Execute(server, context);
                        }                        
                    }
                }
            }
        }
    }
}