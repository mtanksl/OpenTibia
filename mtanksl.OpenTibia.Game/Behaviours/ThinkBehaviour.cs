using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class ThinkBehaviour : Behaviour
    {
        public abstract Promise Update();
    }
}