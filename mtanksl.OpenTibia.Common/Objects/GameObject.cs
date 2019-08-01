using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class GameObject
    {
        private List<IComponent> components = null;

        public T AddComponent<T>() where T : IComponent
        {
            if (components == null)
            {
                components = new List<IComponent>();
            }

            T component = Activator.CreateInstance<T>();

                component.GameObject = this;

            components.Add(component);

            return component;
        }

        public void RemoveComponent(IComponent component)
        {
            if (components == null)
            {
                return;
            }

            component.GameObject = null;

            components.Remove(component);
        }

        public T GetComponent<T>() where T : IComponent
        {
            if (components == null)
            {
                return default(T);
            }

            return components.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetComponents<T>() where T : IComponent
        {
            if (components == null)
            {
                return Enumerable.Empty<T>();
            }

            return components.OfType<T>();
        }
    }
}