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
                new CombineRandomAttackStrategy(false,
                    AttackStrategyFactory.Create(MinMaxAttackType.Melee, 0, 120),
                    AttackStrategyFactory.Create(MinMaxAttackType.FireWave, 100, 170),
                    AttackStrategyFactory.Create(MinMaxAttackType.GreatFireball, 60, 140),
                    AttackStrategyFactory.Create(MinMaxAttackType.SelfHealing, 40, 70) ),
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