using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;

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
            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                playerIdleBehaviour.SetLastAction();
            }

            return Context.AddCommand(new PlayerSayToNpcCommand(Player, Message) );
        }
    }
}