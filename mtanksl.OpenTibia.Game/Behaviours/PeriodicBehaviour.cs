using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class PeriodicBehaviour : Behaviour
    {
        public abstract Promise Update();
    }
}