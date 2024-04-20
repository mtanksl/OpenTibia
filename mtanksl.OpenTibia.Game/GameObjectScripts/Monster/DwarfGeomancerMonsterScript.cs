using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DwarfGeomancerMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Dwarf Geomancer";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new SimpleAttackStrategy(ProjectileType.Poison, MagicEffectType.GreenRings, AnimatedTextColor.Green, 50, 145), 
                    new HealingAttackStrategy(0, 130) ), 
                KeepDistanceWalkStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}