using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Spells
{
    public class WhirlwindThrowSpellPlugin : SpellPlugin
    {
        public WhirlwindThrowSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            if (target != null && player.Tile.Position.IsInRange(target.Tile.Position, 5) && Context.Server.Pathfinding.CanThrow(player.Tile.Position, target.Tile.Position) && Formula.GetKnightWeapon(player) != null)
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            Item itemWeapon = Formula.GetKnightWeapon(player);

            if (itemWeapon.Metadata.WeaponType == WeaponType.Sword)
            {
                var formula = Formula.WhirlwindThrowFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.Sword), itemWeapon.Metadata.Attack.Value);

                return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                   new DamageAttack(ProjectileType.WhirlWindSword, MagicEffectType.GroundShaker, DamageType.Physical, formula.Min, formula.Max) ) );
            }
            else if (itemWeapon.Metadata.WeaponType == WeaponType.Axe)
            {
                var formula = Formula.WhirlwindThrowFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.Axe), itemWeapon.Metadata.Attack.Value);
            
                return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                   new DamageAttack(ProjectileType.WhirlWindAxe, MagicEffectType.GroundShaker, DamageType.Physical, formula.Min, formula.Max) ) );
            }
            else if (itemWeapon.Metadata.WeaponType == WeaponType.Club)
            {
                var formula = Formula.WhirlwindThrowFormula(player.Level, player.Skills.GetClientSkillLevel(Skill.Club), itemWeapon.Metadata.Attack.Value);
                
                return Context.AddCommand(new CreatureAttackCreatureCommand(player, target,

                   new DamageAttack(ProjectileType.WhirlWindClub, MagicEffectType.GroundShaker, DamageType.Physical, formula.Min, formula.Max) ) );
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}