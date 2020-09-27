using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class TradeWithCommand : Command
    {
        public TradeWithCommand(Player player)
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

        protected void TradeWith(Item fromItem, Player toPlayer, Context context)
        {
            Command command = context.TransformCommand(new PlayerTradeWithCommand(Player, fromItem, toPlayer) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}