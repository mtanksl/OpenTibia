using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class GameObject
    {
        private List<Component> components = null;

        public void AddComponent(Component component)
        {
            if (components == null)
            {
                components = new List<Component>();
            }

            component.GameObject = this;

            components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            if (components == null)
            {
                return;
            }

            component.GameObject = null;

            components.Remove(component);
        }

        public T GetComponent<T>()
        {
            if (components == null)
            {
                return default(T);
            }

            return components.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetComponents<T>()
        {
            if (components == null)
            {
                return Enumerable.Empty<T>();
            }

            return components.OfType<T>();
        }
    }
}