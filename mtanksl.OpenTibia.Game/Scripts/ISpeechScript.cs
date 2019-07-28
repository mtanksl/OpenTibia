using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface ISpeechScript : IScript
    {
        bool Execute(Player player, string parameters, Server server, CommandContext context);
    }
}