using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Scripts
{
    public interface ISpeechScript : IScript
    {
        bool OnSpeech(Player player, string parameters, Context context);
    }
}