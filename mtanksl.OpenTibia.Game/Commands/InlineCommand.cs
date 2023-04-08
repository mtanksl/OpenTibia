using System;

namespace OpenTibia.Game.Commands
{
    public class InlineCommand : Command
    {
        private Func<Context, Promise> execute;

        public InlineCommand(Func<Context, Promise> execute)
        {
            this.execute = execute;
        }

        public override Promise Execute(Context context)
        {
            return execute(context);
        }
    }
}