using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class SpellPlugin : Plugin
    {
        public SpellPlugin(Spell spell)
        {
            Spell = spell;
        }

        public Spell Spell { get; }

        public abstract PromiseResult<bool> OnCasting(Player player, Creature target, string message);

        public abstract Promise OnCast(Player player, Creature target, string message);
    }
}