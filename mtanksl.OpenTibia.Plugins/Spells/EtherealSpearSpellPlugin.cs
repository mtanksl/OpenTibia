using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Spells
{
    public class EtherealSpearSpellPlugin : SpellPlugin
    {
        public EtherealSpearSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            if (target != null && player.Tile.Position.IsInRange(target.Tile.Position, 5) && Context.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            var formula = Formula.EtherealSpearFormula(player.Level, player.Skills.GetSkillLevel(Skill.Distance) );

            return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                new DamageAttack(ProjectileType.EthernalSpear, MagicEffectType.GroundShaker, DamageType.Physical, formula.Min, formula.Max) ) );
        }
    }
}