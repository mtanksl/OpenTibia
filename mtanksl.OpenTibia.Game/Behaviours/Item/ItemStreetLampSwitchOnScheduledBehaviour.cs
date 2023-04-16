using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class ItemStreetLampSwitchOnScheduledBehaviour : ScheduledBehaviour
    {
        public ItemStreetLampSwitchOnScheduledBehaviour() : base(18, 00)
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
            return Context.AddCommand(new ItemTransformCommand(item, 1480, 1) );
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}