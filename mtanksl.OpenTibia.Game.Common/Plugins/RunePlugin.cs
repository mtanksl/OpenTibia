using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class RunePlugin : Plugin
    {
        public RunePlugin(Rune rune)
        {
            Rune = rune;
        }

        public Rune Rune { get; }

        public abstract PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item);

        public abstract Promise OnUseRune(Player player, Creature target, Tile tile, Item item);
    }
}