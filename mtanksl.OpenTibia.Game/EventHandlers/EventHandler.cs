using OpenTibia.Common.Events;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler : IEventHandler
    {
        public Guid Token { get; } = Guid.NewGuid();

        public abstract void Execute(object sender, object e);
    }

    public abstract class EventHandler<T> : EventHandler where T : GameEventArgs
    {
        public override void Execute(object sender, object e)
        {
            Execute(sender, (T)e);
        }

        public abstract void Execute(object sender, T e);
    }

    public class InlineEventHandler<T> : EventHandler<T> where T : GameEventArgs
    {
        private Action<object, T> execute;

        public InlineEventHandler(Action<object, T> execute)
        {
            this.execute = execute;
        }

        public override void Execute(object sender, T e)
        {
            execute(sender, e);
        }
    }
}