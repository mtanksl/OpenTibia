using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromInventoryCommand : Command
    {
        public UseItemWithCreatureFromInventoryCommand(Player player, byte fromSlot, ushort itemId, uint toCreatureId)
        {
            Player = player;

            FromSlot = fromSlot;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

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