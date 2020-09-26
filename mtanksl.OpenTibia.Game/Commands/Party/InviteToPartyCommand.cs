using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class InviteToPartyCommand : Command
    {
        public InviteToPartyCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Context context)
        {
            //Arrange
            
            //Act

            //Notify

            base.Execute(context);
        }
    }
}