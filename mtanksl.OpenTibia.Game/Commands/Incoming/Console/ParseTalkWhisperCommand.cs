using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkWhisperCommand : Command
    {
        public ParseTalkWhisperCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // #w <message>

            return Context.AddCommand(new PlayerWhisperCommand(Player, Message) );
        }
    }
}