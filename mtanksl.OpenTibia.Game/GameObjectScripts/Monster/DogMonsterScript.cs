using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DogMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Dog";
            }
        }

        public override void Start(Monster monster)
        {
            base.Start(monster);

            Context.Server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(null, new ApproachWalkStrategy() ) );
        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}