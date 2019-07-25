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
                
        public override void Execute(Server server, CommandContext context)
        {
            SequenceCommand command = new SequenceCommand(

                new StopWalkCommand(Player),

                new StopAttackCommand(Player),

                new StopFollowCommand(Player) );

            command.Completed += (s, e) =>
            {
                //Act

                base.Execute(e.Server, e.Context);
            };

            command.Execute(server, context);
        }
    }
}