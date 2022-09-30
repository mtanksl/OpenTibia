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

        public override void Execute(Context context)
        {
            for (int i = 0; i < commands.Length; i++)
            {
                context.AddCommand(commands[i], ctx =>
                {
                    if (++index == 1)
                    {
                        OnComplete(ctx);
                    }
                } );
            }
        }
    }
}