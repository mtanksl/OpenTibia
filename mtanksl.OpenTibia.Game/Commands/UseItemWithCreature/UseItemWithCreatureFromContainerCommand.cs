using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromContainerCommand : UseItemWithCreatureCommand
    {
        public UseItemWithCreatureFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, uint creatureId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act



            //Notify

            
        }
    }
}