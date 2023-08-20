using System;

namespace OpenTibia.Game.Components
{
    public class PlayerActionDelayBehaviour : DelayBehaviour
    {
        public PlayerActionDelayBehaviour() : base(TimeSpan.FromMilliseconds(200) )
        {

        }
    }
}