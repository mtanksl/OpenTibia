using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class StopCommand : Command
    {
        public StopCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            new StopWalkCommand(Player).Execute(server, context);

            new StopAttackCommand(Player).Execute(server, context);

            new StopFollowCommand(Player).Execute(server, context);

            //Notify

            base.Execute(server, context);
        }
    }
}