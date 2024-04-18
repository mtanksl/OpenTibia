using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkPrivatePlayerToNpcCommand : IncomingCommand
    {
        public ParseTalkPrivatePlayerToNpcCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            return Context.AddCommand(new PlayerSayToNpcCommand(Player, Message) );
        }
    }
}