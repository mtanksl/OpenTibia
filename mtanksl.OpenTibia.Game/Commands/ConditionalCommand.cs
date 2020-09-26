using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionalCommand : Command
    {
        private Func<Context, bool> callback;

        public ConditionalCommand(Func<Context, bool> callback)
        {
            this.callback = callback;
        }

        public override void Execute(Context context)
        {
            if ( callback(context) )
            {
                base.OnCompleted(context);
            }
        }
    }
}