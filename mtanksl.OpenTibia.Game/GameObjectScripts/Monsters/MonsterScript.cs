using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class MonsterScript : GameObjectScript<Monster>
    {
        public override void Start(Monster monster)
        {
            if (monster.Metadata.Voices != null && monster.Metadata.Voices.Items != null && monster.Metadata.Voices.Items.Length > 0)
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new MonsterTalkBehaviour(monster.Metadata.Voices) );
            }

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                AttackStrategyFactory.Create(MinMaxAttackType.Melee, 0, 20),
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