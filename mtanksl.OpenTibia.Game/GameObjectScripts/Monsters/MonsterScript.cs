using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
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
                    MonsterAttackPlugin monsterAttackPlugin = Context.Server.Plugins.GetMonsterAttackPlugin(attack.Name);

                    if (monsterAttackPlugin != null)
                    {
                        attackStrategies.Add(new IntervalAndChanceAttackStrategy(attack.Interval, attack.Chance, new MonsterAttackPluginAttackStrategy(attack.Name, attack.Min, attack.Max, attack.Attributes) ) );
                    }
                }
            }

            if (monster.Metadata.Defenses != null)
            {
                foreach (var defense in monster.Metadata.Defenses)
                {
                    MonsterAttackPlugin monsterAttackPlugin = Context.Server.Plugins.GetMonsterAttackPlugin(defense.Name);

                    if (monsterAttackPlugin != null)
                    {
                        attackStrategies.Add(new IntervalAndChanceAttackStrategy(defense.Interval, defense.Chance, new MonsterAttackPluginAttackStrategy(defense.Name, defense.Min, defense.Max, defense.Attributes) ) );
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

            if (monster.Metadata.TargetMostDamageChance > 0)
            {
                targetStrategies.Add(new IntervalAndChanceTargetStrategy(1000, monster.Metadata.TargetMostDamageChance, MostDamageToAttackerTargetStrategy.Instance) );
            }

            if (monster.Metadata.TargetRandomChance > 0)
            {
                targetStrategies.Add(new IntervalAndChanceTargetStrategy(1000, monster.Metadata.TargetRandomChance, RandomTargetStrategy.Instance) );
            }

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(

                (attackStrategies.Count > 0 ? 
                    new CombineComboRandomAttackStrategy(attackStrategies.ToArray() ) :
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