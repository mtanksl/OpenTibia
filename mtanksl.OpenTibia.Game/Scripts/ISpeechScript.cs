using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts
{
    public interface ISpeechScript : IScript
    {
        bool OnSpeech(Player player, string parameters, Server server, Context context);
    }
}