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
            PlayerSayToNpcEventArgs playerSayToNpcEventArgs = new PlayerSayToNpcEventArgs(Player, Message);

            Context.AddEvent(playerSayToNpcEventArgs);

            return Promise.Completed;
        }
    }
}