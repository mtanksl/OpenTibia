using OpenTibia.Common.Objects;
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
            if (monster.Metadata.Sentences != null)
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new CreatureTalkBehaviour(TimeSpan.FromSeconds(30), TalkType.MonsterSay, monster.Metadata.Sentences) );
            }

            Context.Server.GameObjectComponents.AddComponent(monster, new CreatureWalkBehaviour(new RandomWalkStrategy() ) );
        }

        public override void Stop(Monster monster)
        {

        }
    }
}