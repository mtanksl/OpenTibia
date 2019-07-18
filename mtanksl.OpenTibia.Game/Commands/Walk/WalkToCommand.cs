using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

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

            void Next(int index)
            {
                if (index < MoveDirections.Length)
                {
                    WalkCommand command = new WalkCommand(Player, MoveDirections[index] );

                    command.Completed += (sender, e) =>
                    {
                        Next(index + 1);
                    };

                    command.Execute(server, context);
                }
                else
                {
                    OnCompleted();
                }
            }

            Next(0);

            //Notify
        }

        public EventHandler Completed;

        protected virtual void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }
    }
}