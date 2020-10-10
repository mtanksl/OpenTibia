using OpenTibia.Common.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

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