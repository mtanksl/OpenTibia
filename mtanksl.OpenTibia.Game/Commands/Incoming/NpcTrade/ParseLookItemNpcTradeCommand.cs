using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookItemNpcTradeCommand : Command
    {
        public ParseLookItemNpcTradeCommand(Player player, ushort itemId, byte type)
        {
            Player = player;

            ItemId = itemId;

            Type = type;
        }

        public Player Player { get; set; }

        public ushort ItemId { get; set; }

        public byte Type { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {


                resolve();
            } );
        }
    }
}