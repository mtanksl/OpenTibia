using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class EnergyStrikeSpellPlugin : SpellPlugin
    {
        public EnergyStrikeSpellPlugin(Spell spell) : base(spell)
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

            var formula = Formula.GenericFormula(player.Level, player.Skills.MagicLevel, 1.403, 8, 2.203, 13);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                new SimpleAttack(null, null, AnimatedTextColor.LightBlue, formula.Min, formula.Max) ) );
        }
    }
}