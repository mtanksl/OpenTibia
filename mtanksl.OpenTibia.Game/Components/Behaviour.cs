using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public abstract class Behaviour : Component
    {
        public abstract void Start(Server server);

        public abstract void Stop(Server server);
    }
}