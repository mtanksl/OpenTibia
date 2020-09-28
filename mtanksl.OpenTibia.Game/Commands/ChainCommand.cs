using System;

namespace OpenTibia.Game.Commands
{
    public class ChainCommand : Command
    {
        private Func<Context, Command> callback;

        public ChainCommand(Func<Context, Command> callback)
        {
            this.callback = callback;
        }

        public override void Execute(Context context)
        {
            Command command = callback(context);

            if (command != null)
            {
                command.Completed += (s, e) =>
                {
                    base.OnCompleted(e.Context);
                };

                command.Execute(context);
            }
        }
    }
}