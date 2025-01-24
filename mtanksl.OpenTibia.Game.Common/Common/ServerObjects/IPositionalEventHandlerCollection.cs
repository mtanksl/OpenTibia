using OpenTibia.Common.Objects;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPositionalEventHandlerCollection
    {
        Guid Subscribe(GameObject observer, Type type, Func<Context, object, Promise> execute);

        Guid Subscribe(GameObject observer, Type type, IEventHandler eventHandler);

        Guid Subscribe<T>(GameObject observer, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject observer, IEventHandler<T> eventHandler) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject observer, T e, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(GameObject observer, T e, IEventHandler<T> eventHandler) where T : GameEventArgs;

        bool Unsubscribe(GameObject observer, Guid token);

        IEnumerable<IEventHandler> GetEventHandlers(GameObject eventSource, GameEventArgs e);

        void ClearEventHandlers(GameObject observer);
    }
}