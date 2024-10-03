using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ParseTalkWhisperCommand : IncomingCommand
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

            PlayerIdleBehaviour playerIdleBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerIdleBehaviour>(Player);

            if (playerIdleBehaviour != null)
            {
                playerIdleBehaviour.SetLastAction();
            }

            return Context.AddCommand(new PlayerWhisperCommand(Player, Message) );
        }
    }
}