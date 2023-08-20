using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class GameObjectComponentCollection
    {
        private Dictionary<uint, List<Component> > buckets = new Dictionary<uint, List<Component> >();

        public T AddComponent<T>(GameObject gameObject, T component) where T : Component
        {
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
                        if (components.Remove(_component) )
                        {
                            Behaviour _behaviour = _component as Behaviour;

                            if (_behaviour != null)
                            {
                                _behaviour.Stop();
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
                behaviour.Start();
            }

            return component;
        }

        public bool RemoveComponent(GameObject gameObject, Component component)
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                if (components.Remove(component) )
                {
                    Behaviour behaviour = component as Behaviour;

                    if (behaviour != null)
                    {
                        behaviour.Stop();
                    }

                    if (components.Count == 0)
                    {
                        buckets.Remove(gameObject.Id);
                    }

                    return true;
                }
            }

            return false;
        }

        public T GetComponent<T>(GameObject gameObject) where T : Component
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                return components.OfType<T>().FirstOrDefault();
            }

            return default(T);
        }

        public IEnumerable<T> GetComponents<T>(GameObject gameObject) where T : Component
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                return components.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        public void ClearComponents(GameObject gameObject)
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                foreach (var component in components.ToList() )
                {
                    if (components.Remove(component) )
                    {
                        Behaviour behaviour = component as Behaviour;

                        if (behaviour != null)
                        {
                            behaviour.Stop();
                        }

                        if (components.Count == 0)
                        {
                            buckets.Remove(gameObject.Id);
                        }
                    }
                }                
            }
        }
    }
}