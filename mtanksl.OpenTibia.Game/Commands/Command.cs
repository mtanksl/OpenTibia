using System;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public virtual void Execute(Server server, CommandContext context)
        {
            OnCompleted(new CompletedEventArgs(server, context) );
        }

        public EventHandler<CompletedEventArgs> Completed;

        protected virtual void OnCompleted(CompletedEventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, e);
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