using OpenTibia.Mvc;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class DelayedCommandResult : IActionResult
    {
        private Server server;

        private Context context;

        private Command command;

        private string key;

        private int delay;

        public DelayedCommandResult(Server server, string key, int delay, Context context, Command command)
        {
            this.server = server;

            this.context = context;

            this.command = command;

            this.key = key;

            this.delay = delay;
        }

        public void Execute(Context context)
        {
            server.QueueForExecution(key, delay, context, command, null);
        }
    }
}