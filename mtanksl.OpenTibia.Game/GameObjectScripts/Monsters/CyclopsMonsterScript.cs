using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class CyclopsMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 105) ),
                ApproachWalkStrategy.Instance,
                RandomWalkStrategy.Instance,
                DoNotChangeTargetStrategy.Instance,
                RandomTargetStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}