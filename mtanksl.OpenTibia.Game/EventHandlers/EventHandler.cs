using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler : IEventHandler
    {
        public Guid Token { get; } = Guid.NewGuid();

        public abstract void Handle(Context context, object e);
    }

    public abstract class EventHandler<T> : EventHandler where T : GameEventArgs
    {
        public override void Handle(Context context, object e)
        {
            Handle(context, (T)e);
        }

        public abstract void Handle(Context context, T e);
    }

    public class InlineEventHandler<T> : EventHandler<T> where T : GameEventArgs
    {
        private Action<Context, T> execute;

        public InlineEventHandler(Action<Context, T> execute)
        {
            this.execute = execute;
        }

        public override void Handle(Context context, T e)
        {
            execute(context, e);
        }
    }
}