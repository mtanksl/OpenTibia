using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParsePongCommand : Command
    {
        public ParsePongCommand(Player player)
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