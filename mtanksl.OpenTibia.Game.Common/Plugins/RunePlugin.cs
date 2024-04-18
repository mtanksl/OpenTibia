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


        public static (int Min, int Max) GenericFormula(int level, int magicLevel, double minx, double miny, double maxx, double maxy)
        {
            return ( (int)(level * 0.2 + magicLevel * minx + miny), (int)(level * 0.2 + magicLevel * maxx + maxy) );
        }

        public static (int Min, int Max) GenericFormula(int level, int magicLevel, int @base, int variation)
        {
            var formula = 3 * magicLevel + 2 * level;

            return (formula * (@base - variation) / 100, formula * (@base + variation) / 100);
        }
    }
}