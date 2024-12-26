using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class MonsterScript : GameObjectScript<Monster>
    {
        public override void Start(Monster monster)
        {
            if (monster.Metadata.Sentences != null && monster.Metadata.Sentences.Length > 0)
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new CreatureTalkBehaviour(TalkType.MonsterSay, monster.Metadata.Sentences) );
            }

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new MeleeAttackStrategy(0, 20), 
                ApproachWalkStrategy.Instance,
                RandomWalkStrategy.Instance,
                DoNotChangeTargetStrategy.Instance,
                RandomTargetStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {

        }
    }
}