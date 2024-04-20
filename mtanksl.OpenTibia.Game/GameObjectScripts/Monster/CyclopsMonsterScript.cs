using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class CyclopsMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Cyclops";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(
                new MeleeAttackStrategy(0, 120),
                ApproachWalkStrategy.Instance) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}