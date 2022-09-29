using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class CommandHandlerCollection
    {
        private Dictionary<Type, List<ICommandHandler> > types = new Dictionary<Type, List<ICommandHandler> >();

        public void Add<T>(CommandHandler<T> commandHandler) where T : Command
        {
            List<ICommandHandler> handlers;

            if ( !types.TryGetValue(typeof(T), out handlers) )
            {
                handlers = new List<ICommandHandler>();

                types.Add(typeof(T), handlers);
            }

            handlers.Add(commandHandler);
        }

        public void Remove<T>(CommandHandler<T> commandHandler) where T : Command
        {
            List<ICommandHandler> handlers;

            if ( types.TryGetValue(typeof(T), out handlers) )
            {
                handlers.Remove(commandHandler);

                if (handlers.Count == 0)
                {
                    types.Remove(typeof(T) );
                }
            }
        }

        public bool TryGet(Context context, Command command, out ICommandHandler commandHandler)
        {
            List<ICommandHandler> handlers;

            if ( types.TryGetValue(command.GetType(), out handlers) )
            {
                foreach (var handler in handlers)
                {
                    if ( handler.CanHandle(context, command) )
                    {
                        commandHandler = handler;

                        return true;
                    }
                }
            }

            commandHandler = null;

            return false;
        }
    }
}