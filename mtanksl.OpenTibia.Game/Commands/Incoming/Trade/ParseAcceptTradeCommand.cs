using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseAcceptTradeCommand : Command
    {
        public ParseAcceptTradeCommand(Player player)
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