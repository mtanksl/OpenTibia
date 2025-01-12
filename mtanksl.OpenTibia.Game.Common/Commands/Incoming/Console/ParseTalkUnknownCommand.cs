using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkUnknownCommand : IncomingCommand
    {
        public ParseTalkUnknownCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // #a <message>

            if (Player.Rank == Rank.Gamemaster)
            {


                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}