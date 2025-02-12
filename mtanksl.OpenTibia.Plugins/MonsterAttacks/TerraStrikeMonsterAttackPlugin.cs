﻿using OpenTibia.Common.Structures;

namespace OpenTibia.Plugins.MonsterAttacks
{
    public class TerraStrikeMonsterAttackPlugin : BaseRuneTargetMonsterAttackPlugin
    {
        public TerraStrikeMonsterAttackPlugin() : base(ProjectileType.Poison, MagicEffectType.Carniphilia, DamageType.Earth)
        {

        }
    }
}