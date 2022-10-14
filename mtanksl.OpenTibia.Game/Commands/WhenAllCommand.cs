namespace OpenTibia.Game.Commands
{
    public class WhenAllCommand : Command
    {
        private Command[] commands;

        public WhenAllCommand(params Command[] commands)
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
                        if (++index == commands.Length)
                        {
                            resolve(ctx);
                        }
                    } );
                }
            } );
        }
    }
}