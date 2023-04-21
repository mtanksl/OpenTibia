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
            if (component.IsDestroyed)
            {
                throw new InvalidOperationException("Component is destroyed.");
            }

            List<Component> components;

            if ( !buckets.TryGetValue(gameObject.Id, out components) )
            {
                components = new List<Component>();

                buckets.Add(gameObject.Id, components);
            }

            Behaviour behaviour = component as Behaviour;

            if (behaviour != null)
            {
                if (behaviour.IsUnique)
                {
                    foreach (var _component in components.OfType<T>().ToList() )
                    {
                        if ( !_component.IsDestroyed)
                        {
                            _component.IsDestroyed = true;

                            components.Remove(_component);
                        
                            Behaviour _behaviour = _component as Behaviour;

                            if (_behaviour != null)
                            {
                                _behaviour.Stop(server);
                            }

                            _component.GameObject = null;
                        }
                    }
                }
            }

            components.Add(component);

            component.GameObject = gameObject;

            if (behaviour != null)
            {
                behaviour.Start(server);
            }

            return component;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public bool RemoveComponent(GameObject gameObject, Component component)
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                if ( !component.IsDestroyed)
                {
                    component.IsDestroyed = true;

                    components.Remove(component);
                
                    Behaviour behaviour = component as Behaviour;

                    if (behaviour != null)
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
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                foreach (var component in components.ToList() )
                {
                    if ( !component.IsDestroyed)
                    {
                        component.IsDestroyed = true;

                        components.Remove(component);

                        Behaviour behaviour = component as Behaviour;

                        if (behaviour != null)
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
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                return components.OfType<T>().Where(component => !component.IsDestroyed).FirstOrDefault();
            }

            return default(T);
        }

        /// <exception cref="InvalidOperationException"></exception>

        public IEnumerable<T> GetComponents<T>(GameObject gameObject) where T : Component
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                foreach (var component in components.OfType<T>().ToList() ) 
                {
                    if ( !component.IsDestroyed)
                    {
                        yield return component;
                    }
                }
            }
        }
    }
}