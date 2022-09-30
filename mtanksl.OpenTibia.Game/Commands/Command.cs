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

        public Action<Context> Continuation { get; set; }

        public abstract void Execute(Context context);

        protected virtual void OnComplete(Context context)
        {
            if (Continuation != null)
            {
                Continuation(context);
            }
        }
    }

    public class InlineCommand : Command
    {
        private Action<Context, Action<Context>> execute;

        public InlineCommand(Action<Context, Action<Context> > execute)
        {
            this.execute = execute;
        }

        public override void Execute(Context context)
        {
            execute(context, OnComplete);
        }
    }
}