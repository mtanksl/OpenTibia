using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.GameObjectScripts
{
    public abstract class GameObjectScript<TGameObject> where TGameObject : GameObject
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract void Start(TGameObject gameObject);

        public abstract void Stop(TGameObject gameObject);
    }
}