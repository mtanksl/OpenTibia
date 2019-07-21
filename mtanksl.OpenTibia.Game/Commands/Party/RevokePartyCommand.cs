using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class RevokePartyCommand : Command
    {
        public RevokePartyCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}