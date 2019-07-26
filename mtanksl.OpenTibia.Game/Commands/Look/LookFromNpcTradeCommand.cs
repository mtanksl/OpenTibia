using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromNpcTradeCommand : Command
    {
        public LookFromNpcTradeCommand(Player player, ushort itemId, byte type)
        {
            Player = player;

            ItemId = itemId;

            Type = type;
        }

        public Player Player { get; set; }

        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}