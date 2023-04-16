using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class ItemStreetLampSwitchOffScheduledBehaviour : ScheduledBehaviour
    {
        public ItemStreetLampSwitchOffScheduledBehaviour() : base(06, 00)
        {
            
        }

        private Item item;

        public override void Start(Server server)
        {
            item = (Item)GameObject;

            base.Start(server);
        }

        public override Promise Update()
        {
            return Context.AddCommand(new ItemTransformCommand(item, 1479, 1) );
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}