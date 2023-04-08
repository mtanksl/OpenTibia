using System;

namespace OpenTibia.Game.Commands
{
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