using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class ItemStreetLampSwitchOffScheduledBehaviour : ScheduledBehaviour
    {
        public ItemStreetLampSwitchOffScheduledBehaviour() : base(06, 00)
        {
            
        }

        public override Promise Update()
        {
            Item item = (Item)GameObject;

            return Context.AddCommand(new ItemTransformCommand(item, 1479, 1) );
        }
    }
}