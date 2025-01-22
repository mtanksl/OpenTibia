using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class BehemothMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new RandomAttackStrategy(
                    new ScheduledAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 455) ),
                    new ScheduledAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.BoulderThrow, 0, 200) ) ),
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