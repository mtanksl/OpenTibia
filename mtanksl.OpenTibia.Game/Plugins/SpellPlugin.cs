using mtanksl.OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public abstract class SpellPlugin : Plugin
    {
        public SpellPlugin(Spell spell)
        {
            Spell = spell;
        }

        public Spell Spell { get; }
    }
}