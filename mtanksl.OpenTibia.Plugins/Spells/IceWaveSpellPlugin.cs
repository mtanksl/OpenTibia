using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class IceWaveSpellPlugin : SpellPlugin
    {
        public IceWaveSpellPlugin(Spell spell) : base(spell)
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
                                                      new Offset(0, 1),
                                   new Offset(-1, 2), new Offset(0, 2), new Offset(1, 2),
                                   new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
                new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4),
            };

            var formula = Formula.GenericFormula(player.Level, player.Skills.GetSkillLevel(Skill.MagicLevel), 0.81, 4, 2, 12);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, true, player.Tile.Position, area, null, MagicEffectType.IceArea, 
                        
                new DamageAttack(null, null, DamageType.Ice, formula.Min, formula.Max) ) );
        }
    }
}