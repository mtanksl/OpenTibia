using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class PlayerWalkDelayBehaviour : DelayBehaviour
    {
        public PlayerWalkDelayBehaviour(int executeInMilliseconds) : base("PlayerWalkBehaviour", executeInMilliseconds)
        {
            
        }
    }
}