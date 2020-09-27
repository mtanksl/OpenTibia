using OpenTibia.Common.Objects;

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
                Command command = context.TransformCommand(new WalkToUnknownPathCommand(Player, fromTile) );

                command.Completed += (s, e) =>
                {
                    context.Server.QueueForExecution(Constants.CreatureActionSchedulerEvent(Player), Constants.CreatureActionSchedulerEventDelay, this);
                };

                command.Execute(context);

                return false;
            }

            return true;
        }

        protected void UseItem(Item fromItem, byte? containerId, Context context)
        {
            Command command = context.TransformCommand(new PlayerUseItemCommand(Player, fromItem, containerId) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}