using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class LookFromNpcTradeCommand : LookCommand
    {
        public LookFromNpcTradeCommand(Player player, ushort itemId, byte type) : base(player)
        {
            ItemId = itemId;

            Type = type;
        }

        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            //Act

            //Notify

            base.Execute(server, context);
        }
    }
}