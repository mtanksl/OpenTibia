using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public abstract class Behaviour : Component
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract void Start();

        public abstract void Stop();
    }
}