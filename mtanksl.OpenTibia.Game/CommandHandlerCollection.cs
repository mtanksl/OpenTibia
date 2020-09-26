using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class CommandHandlerCollection
    {
        private Dictionary<Type, List<ICommandHandler>> types = new Dictionary<Type, List<ICommandHandler>>();

        public void Add<T>(CommandHandler<T> commandHandler) where T : Command
        {
            if ( !types.TryGetValue(typeof(T), out var handlers) )
            {
                handlers = new List<ICommandHandler>();

                types.Add(typeof(T), handlers);
            }

            handlers.Add(commandHandler);
        }

        public void Remove<T>(CommandHandler<T> commandHandler) where T : Command
        {
            if ( types.TryGetValue(typeof(T), out var handlers) )
            {
                handlers.Remove(commandHandler);
            }
        }

        public bool TryHandle(Command command, Context context)
        {
            if ( types.TryGetValue(command.GetType(), out var handlers) )
            {
                foreach (var handler in handlers)
                {
                    if ( handler.CanHandle(command, context) )
                    {
                        handler.Handle(command, context);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}