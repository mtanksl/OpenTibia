using OpenTibia.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class GameObject
    {
        public uint Id { get; set; }

        private List<Component> components = null;

        public void AddComponent(Component component)
        {
            if (components == null)
            {
                components = new List<Component>();
            }

            component.GameObject = this;

            components.Add(component);

            OnAdd(component);
        }

        public void RemoveComponent(Component component)
        {
            if (components == null)
            {
                return;
            }

            component.GameObject = null;

            components.Remove(component);

            OnRemove(component);
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

        public event EventHandler<GameObjectComponentsCollectionChangedEventArgs> Add;

        protected virtual void OnAdd(Component component)
        {
            if (Add != null)
            {
                Add(this, new GameObjectComponentsCollectionChangedEventArgs(component) );
            }
        }

        public event EventHandler<GameObjectComponentsCollectionChangedEventArgs> Remove;

        protected virtual void OnRemove(Component component)
        {
            if (Remove != null)
            {
                Remove(this, new GameObjectComponentsCollectionChangedEventArgs(component) );
            }
        }
    }
}