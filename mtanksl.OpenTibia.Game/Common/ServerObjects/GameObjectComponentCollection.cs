using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class GameObjectComponentCollection : IGameObjectComponentCollection
    {
        private Dictionary<uint, List<Component> > buckets = new Dictionary<uint, List<Component> >();

        public T AddComponent<T>(GameObject gameObject, T component, bool isUnique = true) where T : Component
        {
            List<Component> components;

            if ( !buckets.TryGetValue(gameObject.Id, out components) )
            {
                components = new List<Component>();

                buckets.Add(gameObject.Id, components);
            }

            if (isUnique)
            {
                foreach (var _component in components.OfType<T>().ToList() )
                {
                    if (components.Remove(_component) )
                    {
                        if (_component is Behaviour _behaviour)
                        {
                            _behaviour.Stop();
                        }
                    }
                }
            }

            components.Add(component);

            component.GameObject = gameObject;

            if (component is Behaviour behaviour)
            {
                behaviour.Start();
            }

            return component;
        }

        public bool RemoveComponent<T>(GameObject gameObject, T component) where T : Component
        {
            List<Component> components;

            if (buckets.TryGetValue(gameObject.Id, out components) )
            {
                if (components.Remove(component) )
                {
                    if (component is Behaviour behaviour)
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
                        if (component is Behaviour behaviour)
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