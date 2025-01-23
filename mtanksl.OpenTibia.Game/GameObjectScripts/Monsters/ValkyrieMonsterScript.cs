using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class ValkyrieMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 70) ),
                    new IntervalAndChanceAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Spears, 0, 50) ) ), 
                new RunAwayOnLowHealthWalkStrategy(10, ApproachWalkStrategy.Instance),
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