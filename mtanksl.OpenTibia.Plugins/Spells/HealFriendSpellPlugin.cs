using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class HealFriendSpellPlugin : SpellPlugin
    {
        public HealFriendSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            if (target != null && player.Tile.Position.CanHearSay(target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            var formula = Formula.GenericFormula(player.Level, player.Skills.GetSkillLevel(Skill.MagicLevel), 10, 0, 14, 0);

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target, 
                        
                new HealingAttack(MagicEffectType.BlueShimmer, formula.Min, formula.Max) ) );
        }
    }
}