using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemFromContainerCommand : Command
    {
        public UseItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, byte containerId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ContainerId = containerId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ContainerId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Container container = fromItem as Container;

                    if (container != null)
                    {
                        Command command;

                        if (ContainerId == FromContainerId)
                        {
                            command = new ReplaceOrCloseContainerCommand(Player, ContainerId, container);
                        }
                        else
                        {
                            command = new OpenOrCloseContainerCommand(Player, container);
                        }

                        command.Completed += (s, e) =>
                        {
                            //Act

                            base.Execute(server, context);
                        };

                        command.Execute(server, context);
                    }
                    else
                    {
                        //Act

                        ItemUseScript script;

                        if ( !server.ItemUseScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, server, context) )
                        {
                            //Notify

                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                        }
                        else
                        {
                            base.Execute(server, context);
                        }
                    }
                }
            }
        }
    }
}