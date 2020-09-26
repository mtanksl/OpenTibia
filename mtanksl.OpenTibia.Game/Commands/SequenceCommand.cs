namespace OpenTibia.Game.Commands
{
    public class SequenceCommand : Command
    {
        private Command[] commands;

        public SequenceCommand(params Command[] commands)
        {
            this.commands = commands;
        }

        private int index = 0;

        public override void Execute(Context context)
        {
            if (index < commands.Length)
            {
                commands[index].Completed += (s, e) =>
                {
                    index++;

                    Execute(e.Context);
                };

                commands[index].Execute(context);
            }
            else
            {
                base.OnCompleted(context);
            }
        }
    }
}