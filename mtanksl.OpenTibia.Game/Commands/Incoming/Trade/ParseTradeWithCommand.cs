using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseTradeWithCommand : Command
    {
        public ParseTradeWithCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
    }
}