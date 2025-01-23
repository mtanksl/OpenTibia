using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DragonMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 120) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.FireWave, 100, 170) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.GreatFireball, 60, 140) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.SelfHealing, 40, 70) ) ),
                new RunAwayOnLowHealthWalkStrategy(300, ApproachWalkStrategy.Instance),
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