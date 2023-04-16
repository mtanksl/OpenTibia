using OpenTibia.Game.Commands;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler<T> : ICommandHandler<T> where T : Command
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        [DebuggerStepThrough]
        public Promise Handle(Func<Promise> next, Command command)
        {
            return Handle(next, (T)command);
        }

        public abstract Promise Handle(Func<Promise> next, T command);
    }
}