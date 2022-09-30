using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class QuestsCommand : Command
    {
        public QuestsCommand(Player player)
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