using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DragonMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Dragon";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new MeleeAttackStrategy(0, 130),
                    new BeamAttackStrategy(Offset.Wave1133, MagicEffectType.FireArea, AnimatedTextColor.Orange, 100, 170),
                    new AreaAttackStrategy(Offset.Circle5, ProjectileType.Fire, MagicEffectType.FireArea, AnimatedTextColor.Orange, 60, 110),
                    new HealingAttackStrategy(38, 72) ),
                new RunAwayOnLowHealthWalkStrategy(300, ApproachWalkStrategy.Instance) ) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}