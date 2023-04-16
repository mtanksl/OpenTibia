using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class ItemDecayDelayBehaviour : DelayBehaviour
    {
        public ItemDecayDelayBehaviour(int executeInMilliseconds) : base("ItemDecayBehaviour", executeInMilliseconds)
        {
            
        }
    }
}