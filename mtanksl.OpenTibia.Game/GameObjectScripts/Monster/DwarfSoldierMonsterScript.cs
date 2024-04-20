using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DwarfSoldierMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Dwarf Soldier";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new CombineRandomAttackStrategy(
                    new MeleeAttackStrategy(0, 80), 
                    new DistanceAttackStrategy(ProjectileType.Bolt, 0, 40) ), 
                ApproachWalkStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}