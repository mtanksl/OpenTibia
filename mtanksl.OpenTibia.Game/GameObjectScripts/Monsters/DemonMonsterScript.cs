using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DemonMonsterScript : MonsterScript
    {
        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(false,
                    new MeleeAttackStrategy(0, 500),
                    new RuneAreaAttackStrategy(Offset.Circle7, ProjectileType.Fire, MagicEffectType.FireArea, AnimatedTextColor.Orange, 150, 250),
                    new SpellBeamAttackStrategy(Offset.Beam7, MagicEffectType.EnergyArea, AnimatedTextColor.LightBlue, 300, 480),
                    new SpellHealingAttackStrategy(80, 250),
                    new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Fire, MagicEffectType.FirePlume, 1492, 1, AnimatedTextColor.Orange, 20, 20,
                        new DamageCondition(SpecialCondition.Burning, MagicEffectType.FirePlume, AnimatedTextColor.Orange, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) ),
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