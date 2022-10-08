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

        public override Promise Execute(Context context)
        {
            if (index < commands.Length)
            {
                return context.AddCommand(commands[index++] ).Then(ctx =>
                {
                    return Execute(ctx);
                } );
            }

            return Promise.FromResult(context);
        }
    }
}