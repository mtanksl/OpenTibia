using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class WalkToCommand : Command
    {
        public WalkToCommand(Player player, MoveDirection[] moveDirections)
        {
            Player = player;

            MoveDirections = moveDirections;
        }

        public Player Player { get; set; }

        public MoveDirection[] MoveDirections { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            Walk(0, server, context);

            //Notify
        }

        protected void Walk(int index, Server server, CommandContext context)
        {
            WalkCommand command = new WalkCommand(Player, MoveDirections[index] );

            command.Completed += (s, e) =>
            {
                if (index + 1 < MoveDirections.Length)
                {
                    Walk(index + 1, e.Server, e.Context);
                }
                else
                {
                    OnCompleted(e);
                }
            };

            command.Execute(server, context);
        }
    }
}