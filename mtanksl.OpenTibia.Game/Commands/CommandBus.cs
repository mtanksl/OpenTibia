using OpenTibia.Threading;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Commands
{
    public class CommandBus
    {
        private Server server;

        public CommandBus(Server server)
        {
            this.server = server;
        }

        private bool executing = false;

        public void Execute(Command command, Context context)
        {
            if (executing)
            {
                throw new InvalidOperationException();
            }

            executing = true;

            command.Execute(context);

            foreach (var e in command.GetEvents() )
            {
                e.Execute(context);
            }

            context.Response.Flush();

            executing = false;
        }
    }
}