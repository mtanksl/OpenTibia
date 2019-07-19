using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromInventoryCommand : UseItemWithCreatureCommand
    {
        public UseItemWithCreatureFromInventoryCommand(Player player, byte fromSlot, uint creatureId)
        {
            Player = player;

            FromSlot = fromSlot;

            CreatureId = creatureId;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public uint CreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            

            //Act
            
            

            //Notify


        }
    }
}