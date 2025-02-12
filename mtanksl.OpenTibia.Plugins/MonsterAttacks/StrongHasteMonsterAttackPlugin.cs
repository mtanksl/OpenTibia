﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class StrongHasteMonsterAttackPlugin : MonsterAttackPlugin
    {
        public StrongHasteMonsterAttackPlugin()
        {
            
        }

        public override PromiseResult<bool> OnAttacking(Monster attacker, Creature target)
        {
            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnAttack(Monster attacker, Creature target, int min, int max, Dictionary<string, string> attributes)
        {
            var speed = Formula.StrongHasteFormula(attacker.BaseSpeed);

            return Context.Current.AddCommand(new ShowMagicEffectCommand(attacker, MagicEffectType.GreenShimmer) ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureAddConditionCommand(attacker, 
                            
                    new HasteCondition(speed, new TimeSpan(0, 0, 22) ) ) );
            } );  
        }        
    }
}