using OpenTibia.Common.Objects;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectEventHandlerCollection
    {
        Guid Subscribe<T>(GameObject gameObject, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject gameObject, IEventHandler<T> eventHandler) where T : GameEventArgs;

        bool Unsubscribe<T>(GameObject gameObject, Guid token) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject gameObject, T e, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject gameObject, T e, IEventHandler<T> eventHandler) where T : GameEventArgs;

        bool Unsubscribe<T>(GameObject gameObject, T e, Guid token) where T : GameEventArgs;

        IEnumerable<IEventHandler> GetEventHandlers(GameObject gameObject, GameEventArgs e);

        void ClearEventHandlers(GameObject gameObject);    
    }
}