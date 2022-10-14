using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class ParseUseItemCommand : Command
    {
        public ParseUseItemCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
    }
}