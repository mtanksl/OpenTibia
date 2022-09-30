using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class CommandResult<TResult>
    {
        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

        public Action<Context, TResult> ContinueWith { get; set; }

        public abstract void Execute(Context context);

        protected virtual void OnComplete(Context context, TResult result)
        {
            if (ContinueWith != null)
            {
                ContinueWith(context, result);
            }
        }
    }

    public class InlineCommandResult<TResult> : CommandResult<TResult>
    {
        private Action<Context, Action<Context, TResult> > execute;

        public InlineCommandResult(Action<Context, Action<Context, TResult> > execute)
        {
            this.execute = execute;
        }

        public override void Execute(Context context)
        {
            execute(context, OnComplete);
        }
    }
}