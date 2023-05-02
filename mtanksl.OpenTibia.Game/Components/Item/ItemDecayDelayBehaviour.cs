using System;

namespace OpenTibia.Game.Components
{
    public class ItemDecayDelayBehaviour : DelayBehaviour
    {
        public ItemDecayDelayBehaviour(TimeSpan executeIn) : base(executeIn)
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