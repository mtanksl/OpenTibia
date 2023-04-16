using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class PlayerActionDelayBehaviour : DelayBehaviour
    {
        public PlayerActionDelayBehaviour() : base("PlayerActionBehaviour", 200)
        {
            
        }
    }
}