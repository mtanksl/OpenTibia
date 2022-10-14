using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class ComponentCollection
    {
        private Server server;

        public ComponentCollection(Server server)
        {
            this.server = server;
        }

        private Dictionary<uint, List<Component> > buckets = new Dictionary<uint, List<Component> >();

        public T AddComponent<T>(GameObject gameObject, T component) where T : Component
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            if ( !buckets.TryGetValue(gameObject.Id, out var components) )
            {
                components = new List<Component>();

                buckets.Add(gameObject.Id, components);
            }

            component.GameObject = gameObject;

            components.Add(component);

            if (component is Behaviour behaviour)
            {
                behaviour.Start(server);
            }

            return component;
        }

        public void RemoveComponent(GameObject gameObject, Component component)
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            if (buckets.TryGetValue(gameObject.Id, out var components) )
            {
                component.GameObject = null;

                components.Remove(component);

                if (components.Count == 0)
                {
                    buckets.Remove(gameObject.Id);
                }
            }

            if (component is Behaviour behaviour)
            {
                behaviour.Stop(server);
            }
        }

        public T GetComponent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            if (buckets.TryGetValue(gameObject.Id, out var components) )
            {
                return components.OfType<T>().FirstOrDefault();
            }

            return default(T);
        }

        public IEnumerable<T> GetComponents<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            if (buckets.TryGetValue(gameObject.Id, out var components) )
            {
                return components.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            foreach (var bucket in buckets)
            {
                foreach (var component in bucket.Value.OfType<T>() )
                {
                    yield return component;
                }
            }
        }  
    }
}