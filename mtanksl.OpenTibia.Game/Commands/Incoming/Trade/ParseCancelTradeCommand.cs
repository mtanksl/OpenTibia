using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseCancelTradeCommand : Command
    {
        public ParseCancelTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {


                resolve(context);
            } );
        }
    }
}