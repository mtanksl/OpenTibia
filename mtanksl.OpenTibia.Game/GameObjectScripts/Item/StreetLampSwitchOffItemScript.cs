using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class StreetLampSwitchOffItemScript : ItemScript
    {
        public override ushort Key
        {
            get
            {
                return 1480;
            }
        }

        public override void Start(Item item)
        {
            base.Start(item);

            Context.Server.GameObjectComponents.AddComponent(item, new ItemStreetLampSwitchOffScheduledBehaviour() );
        }

        public override void Stop(Item item)
        {
            base.Stop(item);


        }
    }
}