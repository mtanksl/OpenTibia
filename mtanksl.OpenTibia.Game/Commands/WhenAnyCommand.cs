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
                commands[i].Completed += (s, e) =>
                {
                    if (++index == 1)
                    {
                        base.OnCompleted(e.Context);
                    }
                };

                commands[i].Execute(context);
            }
        }
    }
}