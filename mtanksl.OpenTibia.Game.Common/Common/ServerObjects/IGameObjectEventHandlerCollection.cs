using OpenTibia.Common.Objects;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectEventHandlerCollection
    {
        Guid Subscribe(GameObject eventSource, Type type, Func<Context, object, Promise> execute);

        Guid Subscribe(GameObject eventSource, Type type, IEventHandler eventHandler);

        Guid Subscribe<T>(GameObject eventSource, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject eventSource, IEventHandler<T> eventHandler) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject eventSource, T e, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject eventSource, T e, IEventHandler<T> eventHandler) where T : GameEventArgs;

        bool Unsubscribe(Guid token);

        IEnumerable<IEventHandler> GetEventHandlers(GameObject eventSource, GameEventArgs e);

        void ClearEventHandlers(GameObject eventSource);    
    }
}