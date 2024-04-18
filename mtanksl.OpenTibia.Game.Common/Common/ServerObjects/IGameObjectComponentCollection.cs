using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectComponentCollection
    {
        T AddComponent<T>(GameObject gameObject, T component, bool isUnique = true) where T : Component;

        bool RemoveComponent<T>(GameObject gameObject, T component) where T : Component;

        T GetComponent<T>(GameObject gameObject) where T : Component;

        IEnumerable<T> GetComponents<T>(GameObject gameObject) where T : Component;

        void ClearComponents(GameObject gameObject);
    }
}