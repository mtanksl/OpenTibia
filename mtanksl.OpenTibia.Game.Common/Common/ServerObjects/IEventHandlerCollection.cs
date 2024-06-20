using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IEventHandlerCollection
    {
        int Count { get; }

        Guid Subscribe<T>(Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(IEventHandler<T> eventHandler) where T : GameEventArgs;

        bool Unsubscribe<T>(Guid token) where T : GameEventArgs;

        Guid Subscribe<T>(T e, Func<Context, T, Promise> execute) where T : GameEventArgs;

        Guid Subscribe<T>(T e, IEventHandler<T> eventHandler) where T : GameEventArgs;

        bool Unsubscribe<T>(T e, Guid token) where T : GameEventArgs;

        IEnumerable<IEventHandler> GetEventHandlers(GameEventArgs e);

        void Clear();
    }
}