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
            context.AddCommand(commands[index++], ctx => 
            {
                if (index < commands.Length)
                {
                    Execute(ctx);
                }
                else
                {
                    OnComplete(ctx);
                }
            } );
        }
    }
}