using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class SharedExperienceCommand : Command
    {
        public SharedExperienceCommand(Player player, bool enabled)
        {
            Player = player;

            Enabled = enabled;
        }

        public Player Player { get; set; }

        public bool Enabled { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}