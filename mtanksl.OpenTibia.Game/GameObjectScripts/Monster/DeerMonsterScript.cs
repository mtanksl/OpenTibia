using OpenTibia.Common.Objects;

namespace OpenTibia.Game.GameObjectScripts
{
    public class DeerMonsterScript : MonsterScript
    {
        public override string Key
        {
            get
            {
                return "Deer";
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