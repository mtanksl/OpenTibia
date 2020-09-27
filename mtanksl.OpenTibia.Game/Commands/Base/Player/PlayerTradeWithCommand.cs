using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerTradeWithCommand : Command
    {
        public PlayerTradeWithCommand(Player player, Item item, Player toPlayer)
        {
            Player = player;

            Item = item;

            ToPlayer = toPlayer;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public Player ToPlayer { get; set; }

        public override void Execute(Context context)
        {
            base.OnCompleted(context);
        }
    }
}