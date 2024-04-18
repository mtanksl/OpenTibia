using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class FlameStrikeSpellPlugin : SpellPlugin
    {
        public FlameStrikeSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Offset[] area = new Offset[]
            {
                new Offset(0, 1)
            };

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 45, 10);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.FirePlume, 
                        
                new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
        }
    }
}