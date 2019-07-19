using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TradeWithFromInventoryCommand : TradeWithCommand
    {
        public TradeWithFromInventoryCommand(Player player, byte fromSlot, uint creatureId)
        {
            Player = player;

            FromSlot = fromSlot;

            ToCreatureId = creatureId;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null)
            {
                //Act

                
            }
        }
    }
}