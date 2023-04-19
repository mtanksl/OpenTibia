using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkSayCommand : Command
    {
        public ParseTalkSayCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            // #s <message>

            return Context.AddCommand(new PlayerSayCommand(Player, Message) );
        }
    }
}