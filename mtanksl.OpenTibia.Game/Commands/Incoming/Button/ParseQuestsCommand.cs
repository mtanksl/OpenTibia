using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseQuestsCommand : Command
    {
        public ParseQuestsCommand(Player player)
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