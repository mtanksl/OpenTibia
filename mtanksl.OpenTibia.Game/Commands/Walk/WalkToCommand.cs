using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Commands
{
    public class WalkToCommand : Command
    {
        private Server server;

        public WalkToCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public MoveDirection[] MoveDirections { get; set; }
        
        public override void Execute(Context context)
        {
            //Arrange

            //Act

            void Next(int index)
            {
                if (index < MoveDirections.Length)
                {
                    WalkCommand command = new WalkCommand(server)
                    {
                        Player = Player,

                        MoveDirection = MoveDirections[index]
                    };

                    command.Completed += (sender, e) =>
                    {
                        Next(index + 1);
                    };

                    command.Execute(context);
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