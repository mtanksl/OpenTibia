using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DemonMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 500) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.GreatFireball, 150, 250) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.EnergyBeam, 300, 480) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.ManaDrain, 30, 120) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.SelfHealing, 80, 250) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.FireField) ) ),
                ApproachWalkStrategy.Instance,
                RandomWalkStrategy.Instance,
                new RandomChangeTargetStrategy(10 / 100.0),
                RandomTargetStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}