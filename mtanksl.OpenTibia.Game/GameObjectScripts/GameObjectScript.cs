using OpenTibia.Common.Objects;

namespace OpenTibia.Game.GameObjectScripts
{
    public abstract class GameObjectScript<TKey, TGameObject> where TGameObject : GameObject
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract TKey Key { get; }

        public abstract void Start(TGameObject gameObject);

        public abstract void Stop(TGameObject gameObject);
    }
}