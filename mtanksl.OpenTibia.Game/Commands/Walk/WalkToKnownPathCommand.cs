using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class WalkToKnownPathCommand : Command
    {
        public WalkToKnownPathCommand(Player player, MoveDirection[] moveDirections)
        {
            Player = player;

            MoveDirections = moveDirections;
        }

        public Player Player { get; set; }

        public MoveDirection[] MoveDirections { get; set; }


        private int index = 0;

        public override void Execute(Server server, Context context)
        {
            //Arrange

            if (index < MoveDirections.Length)
            {
                WalkCommand command = new WalkCommand(Player, MoveDirections[index++] );

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