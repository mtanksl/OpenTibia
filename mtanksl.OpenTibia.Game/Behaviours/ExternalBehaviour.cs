using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class ExternalBehaviour : Behaviour
    {
        public abstract Promise Update();
    }
}