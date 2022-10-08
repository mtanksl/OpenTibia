using System;

namespace OpenTibia.Game.Commands
{
    public class WhenAnyCommand : Command
    {
        private Command[] commands;

        public WhenAnyCommand(params Command[] commands)
        {
            this.commands = commands;
        }

        private int index = 0;

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    context.AddCommand(commands[i] ).Then(ctx =>
                    {
                        if (++index == 1)
                        {
                            resolve(ctx);
                        }
                    } );
                }
            } );
        }
    }
}