using mtanksl.OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public abstract class RunePlugin : Plugin
    {
        public RunePlugin(Rune rune)
        {
            Rune = rune;
        }

        public Rune Rune { get; }
    }
}