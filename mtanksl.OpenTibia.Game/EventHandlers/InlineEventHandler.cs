using OpenTibia.Game.Events;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.EventHandlers
{
    public class InlineEventHandler<T> : EventHandler<T> where T : GameEventArgs
    {
        private Action<Context, T> execute;

        public InlineEventHandler(Action<Context, T> execute)
        {
            this.execute = execute;
        }

        [DebuggerStepThrough]
        public override void Handle(T e)
        {
            execute(Context, e);
        }
    }
}