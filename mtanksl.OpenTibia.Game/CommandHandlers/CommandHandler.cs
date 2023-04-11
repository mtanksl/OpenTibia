using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandler : ICommandHandler
    {
        public Context Context
        {
            get
            {
                return Context.Current;
            }
        }

        public abstract Promise Handle(Func<Promise> next, Command command);
    }
}