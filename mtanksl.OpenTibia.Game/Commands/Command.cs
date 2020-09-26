using OpenTibia.Common.Events;
using System;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public EventHandler<CommandCompletedEventArgs> Completed;

        protected virtual void OnCompleted(Context context)
        {
            if (Completed != null)
            {
                Completed(this, new CommandCompletedEventArgs(context) );
            }
        }

        public abstract void Execute(Context context);
    }
}