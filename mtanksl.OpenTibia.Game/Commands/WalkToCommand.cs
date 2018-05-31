using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class WalkToCommand : AsyncCommand
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
            void Loop(int index)
            {
                if (index < MoveDirections.Length)
                {
                    var walkCommand = new WalkCommand(server) { Player = Player, MoveDirection = MoveDirections[index] };

                    walkCommand.Complete += (s, e) =>
                    {
                        Loop(index + 1);
                    };

                    walkCommand.Execute(context);
                }
                else
                {
                    OnComplete();
                }
            }

            Loop(0);
        }
    }
}