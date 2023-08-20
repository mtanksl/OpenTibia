using OpenTibia.Common.Objects;

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


        }

        public override void Stop(Monster monster)
        {
            base.Stop(monster);


        }
    }
}