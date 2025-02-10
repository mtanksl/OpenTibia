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
                        attackStrategies.Add(new IntervalAndChanceAttackStrategy(attack.Interval, attack.Chance, attackStrategy) );
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
                        attackStrategies.Add(new IntervalAndChanceAttackStrategy(defense.Interval, defense.Chance, attackStrategy) );
                    }
                }
            }

            List<ITargetStrategy> targetStrategies = new List<ITargetStrategy>();

            if (monster.Metadata.TargetWeakestChance > 0)
            {
                targetStrategies.Add(new IntervalAndChanceTargetStrategy(1000, monster.Metadata.TargetWeakestChance, WeakestTargetStrategy.Instance) );
            }

            if (monster.Metadata.TargetNearestChance > 0)
            {
                targetStrategies.Add(new IntervalAndChanceTargetStrategy(1000, monster.Metadata.TargetNearestChance, NearestTargetStrategy.Instance) );
            }

            if (monster.Metadata.TargetMostDamagedChance > 0)
            {
                targetStrategies.Add(new IntervalAndChanceTargetStrategy(1000, monster.Metadata.TargetMostDamagedChance, MostDamagedByAttackerTargetStrategy.Instance) );
            }

            if (monster.Metadata.TargetRandomChance > 0)
            {
                targetStrategies.Add(new IntervalAndChanceTargetStrategy(1000, monster.Metadata.TargetRandomChance, RandomTargetStrategy.Instance) );
            }

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(

                (attackStrategies.Count > 0 ? 
                    new CombineRandomAttackStrategy(attackStrategies.ToArray() ) :
                    DoNotAttackStrategy.Instance),

                (monster.Metadata.Speed > 0 ?
                    (monster.Metadata.RunOnHealth > 0 ?
                        new RunAwayOnLowHealthWalkStrategy(monster.Metadata.RunOnHealth, (monster.Metadata.TargetDistance > 1 ? 
                            new KeepDistanceWalkStrategy(monster.Metadata.TargetDistance) : 
                            ApproachWalkStrategy.Instance) ) :
                        (monster.Metadata.TargetDistance > 1 ? 
                            new KeepDistanceWalkStrategy(monster.Metadata.TargetDistance) : 
                            ApproachWalkStrategy.Instance) ) :
                    DoNotWalkStrategy.Instance),

                (monster.Metadata.Speed > 0 ?
                    RandomWalkStrategy.Instance : 
                    DoNotWalkStrategy.Instance),

                ( (monster.Metadata.ChangeTargetInterval > 0 && monster.Metadata.ChangeTargetChance > 0) ? 
                    new IntervalAndChanceChangeTargetStrategy(monster.Metadata.ChangeTargetInterval, monster.Metadata.ChangeTargetChance, DoChangeTargetStrategy.Instance) : 
                    DoNotChangeTargetStrategy.Instance),

                (targetStrategies.Count > 0 ? 
                    new CombineRandomTargetStrategy(targetStrategies.ToArray() ) : 
                    RandomTargetStrategy.Instance) ) );
        }

        public override void Stop(Monster monster)
        {

        }
    }
}