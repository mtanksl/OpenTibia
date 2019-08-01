using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class JoinPartyCommand : Command
    {
        public JoinPartyCommand(Player player, uint creatureId)
        {
            Player = player;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange
            
            //Act
            
            //Notify
                        
            base.Execute(server, context);
        }
    }
}