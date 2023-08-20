using OpenTibia.Common.Objects;

namespace OpenTibia.Game.GameObjectScripts
{
    public class ItemScript : GameObjectScript<ushort, Item>
    {
        public override ushort Key
        {
            get
            {
                return 0;
            }
        }

        public override void Start(Item item)
        {

        }

        public override void Stop(Item item)
        {

        }
    }
}