using OpenTibia.Game;

namespace mtanksl.OpenTibia.Game.Plugins
{
    public abstract class Plugin
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