using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ParsePongCommand : Command
    {
        public ParsePongCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            PlayerPingBehaviour playerPingBehaviour = Context.Server.Components.GetComponent<PlayerPingBehaviour>(Player);

            if (playerPingBehaviour != null)
            {
                playerPingBehaviour.SetLastPingResponse();
            }

            return Promise.Completed;
        }
    }
}