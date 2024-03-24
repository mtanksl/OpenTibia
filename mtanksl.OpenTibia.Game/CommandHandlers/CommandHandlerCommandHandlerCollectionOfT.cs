using OpenTibia.Game.Commands;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandHandlerCommandHandlerCollection<T> : CommandHandler<T> where T : Command
    {
        public CommandHandlerCommandHandlerCollection()
        {
            CommandHandlers = new CommandHandlerCollection();
        }

        public CommandHandlerCollection CommandHandlers { get; }

        public override Promise Handle(Func<Promise> next, T command)
        {
            if (CanHandle(command) )
            {
                var commandHandlers = CommandHandlers
                    .GetCommandHandlers(command)
                    .GetEnumerator();

                [DebuggerStepThrough] Promise Next()
                {
                    if (commandHandlers.MoveNext() )
                    {
                        var commandHandler = commandHandlers.Current;

                        return commandHandler.Handle(Next, command);
                    }

                    return next();
                }

                return Next();
            }

            return next();
        }

        public abstract bool CanHandle(T command);
    }
}