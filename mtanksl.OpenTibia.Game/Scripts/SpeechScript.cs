using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public abstract class SpeechScript : IScript
    {
        public abstract void Register(Server server);

        public abstract bool Execute(Player player, string message, Server server, CommandContext context);
    }
}