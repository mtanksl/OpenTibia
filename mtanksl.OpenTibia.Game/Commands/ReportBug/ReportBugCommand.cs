using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ReportBugCommand : Command
    {
        public ReportBugCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act



            //Notify

            
        }
    }
}