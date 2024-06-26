﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class ItemStreetLampSwitchOffScheduledBehaviour : TibiaClockScheduledBehaviour
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