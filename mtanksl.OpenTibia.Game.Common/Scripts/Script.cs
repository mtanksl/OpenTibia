using OpenTibia.Game.Common;

namespace OpenTibia.Game.Scripts
{
    public abstract class Script
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