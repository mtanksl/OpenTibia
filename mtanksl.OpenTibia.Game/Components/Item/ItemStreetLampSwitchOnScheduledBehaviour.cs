using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class ItemStreetLampSwitchOnScheduledBehaviour : ScheduledBehaviour
    {
        public ItemStreetLampSwitchOnScheduledBehaviour() : base(18, 00)
        {
            
        }

        public override Promise Update()
        {
            Item item = (Item)GameObject;

            return Context.AddCommand(new ItemTransformCommand(item, 1480, 1) );
        }
    }
}