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

        public abstract PromiseResult<TResult> Execute(Context context);
    }

    public class InlineCommandResult<TResult> : CommandResult<TResult>
    {
        private Func<Context, PromiseResult<TResult> > execute;

        public InlineCommandResult(Func<Context, PromiseResult<TResult> > execute)
        {
            this.execute = execute;
        }

        public override PromiseResult<TResult> Execute(Context context)
        {
            return execute(context);
        }
    }
}