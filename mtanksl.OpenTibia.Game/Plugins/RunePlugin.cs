using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
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

        public static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }
    }
}