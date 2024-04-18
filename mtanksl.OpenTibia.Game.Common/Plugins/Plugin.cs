using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
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

        public virtual void Start()
        {

        }

        public virtual void Stop()
        {
            
        }
    }  
}