using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class AcceptTradeCommand : Command
    {
        public AcceptTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            OnComplete(context);
        }
    }
}