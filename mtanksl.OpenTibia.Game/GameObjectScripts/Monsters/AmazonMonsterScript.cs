using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class AmazonMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new RandomAttackStrategy(
                    new ScheduledAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 45) ),
                    new ScheduledAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.ThrowsKnives, 0, 40) ) ),
                KeepDistanceWalkStrategy.Instance,
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