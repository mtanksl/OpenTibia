using OpenTibia.Game.Commands;
using System;
using System.Diagnostics;

namespace OpenTibia.Game.CommandHandlers
{
    public abstract class CommandResultHandlerCommandHandlerCollection<TResult, T> : CommandResultHandler<TResult, T> where T : CommandResult<TResult>
    {
        public CommandResultHandlerCommandHandlerCollection()
        {
            CommandHandlers = new CommandHandlerCollection();
        }

        public CommandHandlerCollection CommandHandlers { get; set; }

        public override PromiseResult<TResult> Handle(Func<PromiseResult<TResult>> next, T command)
        {
            if (CanHandle(command) )
            {
                var commandHandlers = CommandHandlers
                    .GetCommandResultHandlers(command)
                    .GetEnumerator();

                [DebuggerStepThrough] PromiseResult<TResult> Next()
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