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