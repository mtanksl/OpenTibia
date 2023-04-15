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

        /// <exception cref="InvalidOperationException"></exception>

        public T AddComponent<T>(GameObject gameObject, T component) where T : Component
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            List<Component> components;

            if ( !buckets.TryGetValue(gameObject.Id, out components) )
            {
                components = new List<Component>();

                buckets.Add(gameObject.Id, components);
            }

            components.Add(component);

            component.GameObject = gameObject;

            if (component is Behaviour behaviour)
            {
                behaviour.Start(server);
            }

            return component;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public bool RemoveComponent(GameObject gameObject, Component component)
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                if (components.Remove(component) )
                {
                    if (component is Behaviour behaviour)
                    {
                        behaviour.Stop(server);
                    }

                    component.GameObject = null;

                    if (components.Count == 0)
                    {
                        buckets.Remove(gameObject.Id);
                    }

                    return true;
                }
            }

            return false;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public void ClearComponents(GameObject gameObject)
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                foreach (var component in components.ToList() )
                {
                    if (components.Remove(component) )
                    {
                        if (component is Behaviour behaviour)
                        {
                            behaviour.Stop(server);
                        }

                        component.GameObject = null;

                        if (components.Count == 0)
                        {
                            buckets.Remove(gameObject.Id);
                        }
                    }
                }                
            }
        }

        /// <exception cref="InvalidOperationException"></exception>

        public T GetComponent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                return components.OfType<T>().FirstOrDefault();
            }

            return default(T);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public IEnumerable<T> GetComponents<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.Id == 0)
            {
                throw new InvalidOperationException("GameObject is not initialized.");
            }

            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                return components.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        public IEnumerable<T2> GetComponentsOfType<T1, T2>() where T1 : GameObject 
                                                             where T2 : Component
        {
            foreach (var component in buckets.SelectMany(b => b.Value).Where(c => c.GameObject is T1 && c is T2) )
            {
                yield return (T2)component;
            }
        }
    }
}