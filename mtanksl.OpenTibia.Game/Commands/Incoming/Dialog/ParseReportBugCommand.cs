using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseReportBugCommand : Command
    {
        public ParseReportBugCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            return Promise.Completed;
        }
    }
}