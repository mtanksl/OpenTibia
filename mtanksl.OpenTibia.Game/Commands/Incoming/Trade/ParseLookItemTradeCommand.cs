using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookItemTradeCommand : Command
    {
        public ParseLookItemTradeCommand(Player player, byte windowId, byte index)
        {
            WindowId = windowId;

            Index = index;
        }

        public Player Player { get; set; }

        public byte WindowId { get; set; }

        public byte Index { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {


                resolve(context);
            } );
        }
    }
}