namespace OpenTibia.Game.Commands
{
    public class SequenceCommand : Command
    {
        public SequenceCommand(params Command[] commands)
        {
            Commands = commands;
        }

        public Command[] Commands { get; set; }


        private int index = 0;

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            if (index < Commands.Length)
            {
                Command command = Commands[index++];

                command.Completed += (s, e) =>
                {
                    Execute(e.Server, e.Context);
                };

                command.Execute(server, context);
            }
            else
            {
                //Act

                base.Execute(server, context);
            }
        }
    }
}