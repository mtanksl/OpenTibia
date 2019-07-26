using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromContainerCommand : Command
    {
        public UseItemWithCreatureFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, uint toCreatureId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = server.Map.GetCreature(ToCreatureId);

                    if (toCreature != null)
                    {
                        if ( !fromItem.Metadata.Flags.Is(ItemMetadataFlags.Useable) )
                        {
                            context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
                        }
                        else
                        {
                            //Act

                            ItemUseWithCreatureScript script;

                            if ( !server.ItemUseWithCreatureScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.Execute(Player, fromItem, toCreature, server, context) )
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
}