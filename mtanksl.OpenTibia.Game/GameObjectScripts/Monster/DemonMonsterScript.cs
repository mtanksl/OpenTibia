using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DemonMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Demon";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new MeleeAttackStrategy(0, 500),
                    new AreaAttackStrategy(Offset.Circle7, ProjectileType.Fire, MagicEffectType.FireArea, AnimatedTextColor.Orange, 150, 250),
                    new BeamAttackStrategy(Offset.Beam7, MagicEffectType.EnergyArea, AnimatedTextColor.LightBlue, 300, 480),
                    new HealingAttackStrategy(80, 250) ),
                ApproachWalkStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}