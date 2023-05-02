using System;

namespace OpenTibia.Game.Components
{
    public class PlayerWalkDelayBehaviour : DelayBehaviour
    {
        public PlayerWalkDelayBehaviour(TimeSpan executeIn) : base(executeIn)
        {
            
        }

        public override bool IsUnique
        {
            get
            {
                return true;
            }
        }
    }
}