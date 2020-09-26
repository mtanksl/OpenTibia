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

        protected bool IsNextTo(Tile fromTile, Context context)
        {
            if ( !Player.Tile.Position.IsNextTo(fromTile.Position) )
            {
                WalkToUnknownPathCommand walkToUnknownPathCommand = new WalkToUnknownPathCommand(Player, fromTile);

                walkToUnknownPathCommand.Completed += (s, e) =>
                {
                    context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                };

                walkToUnknownPathCommand.Execute(context);

                return false;
            }

            return true;
        }

        protected void UseItem(Item fromItem, Context context)
        {
            Container container = fromItem as Container;

            if (container != null)
            {
                new ContainerOpenOrCloseCommand(Player, container).Execute(context);

                base.OnCompleted(context);
            }
            else
            {
                Command command = context.AddCommand(new PlayerUseItemCommand(Player, fromItem) );

                command.Completed += (s, e) =>
                {
                    base.OnCompleted(context);
                };

                command.Execute(context);
            }
        }

        protected void UseItem(Item fromItem, byte fromContainerId, byte containerId, Context context)
        {
            Container container = fromItem as Container;

            if (container != null)
            {
                if (fromContainerId == containerId)
                {
                    new ContainerReplaceOrCloseCommand(Player, containerId, container).Execute(context);
                }
                else
                {
                    new ContainerOpenOrCloseCommand(Player, container).Execute(context);
                }

                base.OnCompleted(context);
            }
            else
            {
                Command command = context.AddCommand(new PlayerUseItemCommand(Player, fromItem) );

                command.Completed += (s, e) =>
                {
                    base.OnCompleted(context);
                };

                command.Execute(context);
            }
        }
    }
}