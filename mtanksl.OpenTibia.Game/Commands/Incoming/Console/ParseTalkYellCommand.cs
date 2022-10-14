using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkYellCommand : Command
    {
        public ParseTalkYellCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute(Context context)
        {
            return context.AddCommand(new PlayerYellCommand(Player, Message) );
        }
    }
}