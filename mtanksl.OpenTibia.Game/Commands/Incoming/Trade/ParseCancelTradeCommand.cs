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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {


                resolve(context);
            } );
        }
    }
}