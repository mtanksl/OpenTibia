using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.EventHandlers
{
    public class InlineEventHandler<T> : EventHandler<T> where T : GameEventArgs
    {
        private Action<Context, T> execute;

        public InlineEventHandler(Action<Context, T> execute)
        {
            this.execute = execute;
        }

        public override void Handle(T e)
        {
            execute(Context, e);
        }
    }
}