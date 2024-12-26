using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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
                    new MeleeAttackStrategy(0, 12),
                    new SpellBeamAttackStrategy(Offset.Wave1133355, MagicEffectType.FireArea, AnimatedTextColor.Orange, 100, 170),
                    new RuneAreaAttackStrategy(Offset.Circle5, ProjectileType.Fire, MagicEffectType.FireArea, AnimatedTextColor.Orange, 60, 140),
                    new SpellHealingAttackStrategy(40, 70) ),
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