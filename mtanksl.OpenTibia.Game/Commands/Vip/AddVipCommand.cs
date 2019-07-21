using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class AddVipCommand : Command
    {
        public AddVipCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}