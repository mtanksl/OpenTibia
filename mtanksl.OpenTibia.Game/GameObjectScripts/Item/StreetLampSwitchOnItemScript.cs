using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class StreetLampSwitchOnItemScript : ItemScript
    {
        public override ushort Key
        {
            get
            {
                return 1479;
            }
        }

        public override void Start(Item item)
        {
            base.Start(item);

            Context.Server.GameObjectComponents.AddComponent(item, new ItemStreetLampSwitchOnScheduledBehaviour() );
        }

        public override void Stop(Item item)
        {
            base.Stop(item);


        }
    }
}