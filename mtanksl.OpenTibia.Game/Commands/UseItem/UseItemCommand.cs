using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Scripts;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class UseItemCommand : Command
    {
        public UseItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected bool IsNextTo(Tile fromTile, Server server, CommandContext context)
        {
            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
            {
                WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                walkToUnknownPathCommand.Completed += (s, e) =>
                {
                    server.QueueForExecution(Constants.PlayerSchedulerEvent(Player), Constants.PlayerSchedulerEventDelay, this);
                };

                walkToUnknownPathCommand.Execute(server, context);

                return false;
            }

            return true;
        }

        protected void UseItem(Item fromItem, Server server, CommandContext context)
        {
            Container container = fromItem as Container;

            if (container != null)
            {
                new ContainerOpenOrCloseCommand(Player, container).Execute(server, context);

                base.Execute(server, context);
            }
            else
            {
                IItemUseScript script;

                if ( !server.Scripts.ItemUseScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.OnItemUse(Player, fromItem, server, context) )
                {
                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
                }
                else
                {
                    base.Execute(server, context);
                }
            }
        }

        protected void UseItem(Item fromItem, byte fromContainerId, byte containerId, Server server, CommandContext context)
        {
            Container container = fromItem as Container;

            if (container != null)
            {
                if (fromContainerId == containerId)
                {
                    new ContainerReplaceOrCloseCommand(Player, containerId, container).Execute(server, context);
                }
                else
                {
                    new ContainerOpenOrCloseCommand(Player, container).Execute(server, context);
                }

                base.Execute(server, context);
            }
            else
            {
                IItemUseScript script;

                if ( !server.Scripts.ItemUseScripts.TryGetValue(fromItem.Metadata.OpenTibiaId, out script) || !script.OnItemUse(Player, fromItem, server, context) )
                {
                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisItem) );
                }
                else
                {
                    base.Execute(server, context);
                }
            }
        }
    }
}