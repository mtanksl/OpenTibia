using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class IgnoreCommand : IncomingCommand
    {
        public IgnoreCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {                       
            return Promise.Completed;
        }
    }
}