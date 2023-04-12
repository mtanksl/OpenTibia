using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseNpcTradeCommand : Command
    {
        public ParseCloseNpcTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            return Promise.Completed;
        }
    }
}