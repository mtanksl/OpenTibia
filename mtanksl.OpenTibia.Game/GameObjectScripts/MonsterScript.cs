﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.GameObjectScripts
{
    public class MonsterScript : GameObjectScript<string, Monster>
    {
        public override string Key
        {
            get
            {
                return "";
            }
        }

        public override void Start(Monster monster)
        {
            if (monster.Metadata.Sentences != null && monster.Metadata.Sentences.Length > 0)
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new CreatureTalkBehaviour(TalkType.MonsterSay, monster.Metadata.Sentences) );
            }

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(new MeleeAttackStrategy(0, 20, TimeSpan.FromSeconds(2) ), new FollowWalkStrategy() ) );
        }

        public override void Stop(Monster monster)
        {

        }
    }
}