using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Collections.Generic;

namespace OpenTibia.Game.GameObjectScripts
{
    public class MonsterScript : GameObjectScript<Monster>
    {
        public override void Start(Monster monster)
        {
            if (monster.Metadata.Voices != null)
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new MonsterTalkBehaviour(monster.Metadata.Voices) );
            }

            List<IAttackStrategy> attackStrategies = new List<IAttackStrategy>();

            if (monster.Metadata.Attacks != null)
            {
                foreach (var attack in monster.Metadata.Attacks)
                {
                    IAttackStrategy attackStrategy = AttackStrategyFactory.Create(attack.Name, attack.Min, attack.Max);

                    if (attackStrategy != null)
                    {
                        attackStrategies.Add(new ScheduledAttackStrategy(attack.Interval, attack.Chance, attackStrategy) );
                    }
                }
            }

            if (monster.Metadata.Defenses != null)
            {
                foreach (var defense in monster.Metadata.Defenses)
                {
                    IAttackStrategy attackStrategy = AttackStrategyFactory.Create(defense.Name, defense.Min, defense.Max);

                    if (attackStrategy != null)
                    {
                        attackStrategies.Add(new ScheduledAttackStrategy(defense.Interval, defense.Chance, attackStrategy) );
                    }
                }
            }

            if (attackStrategies.Count > 0)
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                    new RandomAttackStrategy(attackStrategies.ToArray() ),
                    ApproachWalkStrategy.Instance,
                    RandomWalkStrategy.Instance,
                    DoNotChangeTargetStrategy.Instance,
                    RandomTargetStrategy.Instance) );
            }
            else
            {
                Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                    new ScheduledAttackStrategy(2000, 90, AttackStrategyFactory.Create(AttackType.Melee, 0, 20) ),
                    ApproachWalkStrategy.Instance,
                    RandomWalkStrategy.Instance,
                    DoNotChangeTargetStrategy.Instance,
                    RandomTargetStrategy.Instance));
            }
        }

        public override void Stop(Monster monster)
        {

        }
    }
}