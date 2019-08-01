using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PongCommand : Command
    {
        public PongCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}