using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public abstract class BaseSpellAreaMonsterAttackPlugin : MonsterAttackPlugin
    {
        private Offset[] area;

        private MagicEffectType? magicEffectType;

        private DamageType damageType;

        private Condition condition;

        public BaseSpellAreaMonsterAttackPlugin(Offset[] area, MagicEffectType? magicEffectType, DamageType damageType)

            :this(area, magicEffectType, damageType, null)
        {


        }
        public BaseSpellAreaMonsterAttackPlugin(Offset[] area, MagicEffectType? magicEffectType, DamageType damageType, Condition condition)
        {
            this.area = area;

            this.magicEffectType = magicEffectType;

            this.damageType = damageType;

            this.condition = condition;
        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            return Context.Current.AddCommand(new CreatureAttackAreaCommand(attacker, false, attacker.Tile.Position, area, null, magicEffectType, 
                        
                new DamageAttack(null, null, damageType, min, max),
                
                condition) );
        }
    }
}