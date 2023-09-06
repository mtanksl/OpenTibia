using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkPrivatePlayerToNpcCommand : Command
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