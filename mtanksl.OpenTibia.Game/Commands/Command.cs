using System;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public EventHandler<CompletedEventArgs> Completed;

        public virtual void Execute(Server server, CommandContext context)
        {
            if (Completed != null)
            {
                Completed(this, new CompletedEventArgs(server, context) );
            }
        }
    }

    public class CompletedEventArgs : EventArgs
    {
        public CompletedEventArgs(Server server, CommandContext context)
        {
            Server = server;

            Context = context;
        }

        public Server Server { get; }

        public CommandContext Context { get; }
    }
}