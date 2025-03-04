using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public abstract class BaseMeleeMonsterAttackPlugin : MonsterAttackPlugin
    {
        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        public BaseMeleeMonsterAttackPlugin(MagicEffectType? magicEffectType, DamageType damageType)
        {
            this.magicEffectType = magicEffectType;

            this.damageType = damageType;
        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            if (attacker.Tile.Position.IsNextTo(target.Tile.Position) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            return Context.Current.AddCommand(new CreatureAttackCreatureCommand(attacker, target,

                new DamageAttack(null, magicEffectType, damageType, min, max, true) ) );
        }
    }
}