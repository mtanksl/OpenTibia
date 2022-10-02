using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Common.Events
{
    public class GameObjectComponentsCollectionChangedEventArgs : EventArgs
    {
        public GameObjectComponentsCollectionChangedEventArgs(Component component)
        {
            Component = component;
        }

        public Component Component { get; }
    }
}