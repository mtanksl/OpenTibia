using OpenTibia.Mvc;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class CommandResult : IActionResult
    {
        private Server server;

        private Context context;

        private Command command;

        public CommandResult(Server server, Context context, Command command)
        {
            this.server = server;

            this.context = context;

            this.command = command;
        }

        public void Execute(Context context)
        {
            server.QueueForExecution(context, command, null);
        }
    }
}