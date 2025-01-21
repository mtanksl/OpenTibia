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
                new CombineRandomAttackStrategy(false,
                    AttackStrategyFactory.Create(MinMaxAttackType.Melee, 0, 500),
                    AttackStrategyFactory.Create(MinMaxAttackType.GreatFireball, 150, 250),
                    AttackStrategyFactory.Create(MinMaxAttackType.EneryBeam, 300, 480),
                    AttackStrategyFactory.Create(MinMaxAttackType.ManaDrain, 30, 120),
                    AttackStrategyFactory.Create(MinMaxAttackType.SelfHealing, 80, 250),
                    AttackStrategyFactory.Create(AttackType.FireField) ),
                ApproachWalkStrategy.Instance,
                RandomWalkStrategy.Instance,
                new RandomChangeTargetStrategy(10.0 / 100),
                RandomTargetStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}