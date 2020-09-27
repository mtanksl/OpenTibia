using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class CommandHandlerCollection
    {
        private Server server;

        public CommandHandlerCollection(Server server)
        {
            this.server = server;
        }

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

        public bool TryHandle(Command command, out Command result)
        {
            if ( types.TryGetValue(command.GetType(), out var handlers) )
            {
                foreach (var handler in handlers)
                {
                    if ( handler.CanHandle(command, server) )
                    {
                        result = handler.Handle(command, server);

                        return true;
                    }
                }
            }

            result = null;

            return false;
        }
    }
}