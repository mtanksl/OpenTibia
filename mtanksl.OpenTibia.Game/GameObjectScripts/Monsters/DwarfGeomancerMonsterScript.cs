using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DwarfGeomancerMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 100) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.ManaDrain, 25, 80) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Stalagmite, 50, 110) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.SelfHealing, 25, 130) ) ), 
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