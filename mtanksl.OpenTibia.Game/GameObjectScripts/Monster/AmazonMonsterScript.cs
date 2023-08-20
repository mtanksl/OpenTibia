﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.GameObjectScripts
{
    public class AmazonMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Amazon";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(new DistanceAttackStrategy(ProjectileType.ThrowingKnife, 0, 20, TimeSpan.FromSeconds(2) ), new KeepDistanceWalkStrategy(3) ) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}