using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Commands
{
    public class PlayerSayToNpcCommand : Command
    {
        public PlayerSayToNpcCommand(Player player, string message)
        {
            Player = player;

            Message = message;
        }

        public Player Player { get; set; }

        public string Message { get; set; }

        public override Promise Execute()
        {
            Context.AddEvent(new PlayerSayToNpcEventArgs(Player, Message) );

            return Promise.Completed;
        }
    }
}