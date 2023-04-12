using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.EventHandlers
{
    public class InlineEventHandler<T> : EventHandler<T> where T : GameEventArgs
    {
        private Func<Context, T, Promise> execute;

        public InlineEventHandler(Func<Context, T, Promise> execute)
        {
            this.execute = execute;
        }

        [DebuggerStepThrough]
        public override Promise Handle(T e)
        {
            return execute(Context, e);
        }
    }
}