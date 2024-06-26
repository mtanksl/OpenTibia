﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class ItemStreetLampSwitchOnScheduledBehaviour : TibiaClockScheduledBehaviour
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