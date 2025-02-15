using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class GreatEnergyBeamSpellPlugin : SpellPlugin
    {
        public GreatEnergyBeamSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            Offset[] area = new Offset[]
            {
                new Offset(0, 1),
                new Offset(0, 2),
                new Offset(0, 3),
                new Offset(0, 4),
                new Offset(0, 5),
                new Offset(0, 6),
                new Offset(0, 7)
            };

            var formula = Formula.GenericFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.MagicLevel), 4, 0, 7, 0);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.EnergyArea, 
                        
                new DamageAttack(null, null, DamageType.Energy, formula.Min, formula.Max) ) );
        }
    }
}