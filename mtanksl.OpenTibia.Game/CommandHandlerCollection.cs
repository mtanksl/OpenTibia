using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class CommandHandlerCollection
    {
        private Dictionary<Type, List<object> > types = new Dictionary<Type, List<object> >();

        public void Add<T>(CommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( CommandHandler<> ).MakeGenericType(typeof(T) );
           
            if ( !types.TryGetValue(type, out var handlers) )
            {
                handlers = new List<object>();

                types.Add(type, handlers);
            }

            handlers.Add(commandHandler);
        }

        public void Add<T, TResult>(CommandHandlerResult<T, TResult> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(typeof(T), typeof(TResult) );

            if ( !types.TryGetValue(type, out var handlers) )
            {
                handlers = new List<object>();

                types.Add(type, handlers);
            }

            handlers.Add(commandHandler);
        }

        public void Remove<T>(CommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( CommandHandler<> ).MakeGenericType(typeof(T) );
           
            if ( types.TryGetValue(type, out var handlers) )
            {
                handlers.Remove(commandHandler);

                if (handlers.Count == 0)
                {
                    types.Remove(type);
                }
            }
        }

        public void Remove<T, TResult>(CommandHandlerResult<T, TResult> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(typeof(T), typeof(TResult) );

            if ( types.TryGetValue(type, out var handlers) )
            {
                handlers.Remove(commandHandler);

                if (handlers.Count == 0)
                {
                    types.Remove(type);
                }
            }
        }

        public bool TryGet(Context context, Command command, out ICommandHandler result)
        {
            var type = typeof( CommandHandler<> ).MakeGenericType(command.GetType() );

            if ( types.TryGetValue(type, out var handlers) )
            {
                foreach (ICommandHandler commandHandler in handlers)
                {
                    if (commandHandler.CanHandle(context, command) )
                    {
                        result = commandHandler;

                        return true;
                    }
                }
            }

            result = null;

            return false;
        }

        public bool TryGet<TResult>(Context context, CommandResult<TResult> command, out ICommandHandlerResult<TResult> result)
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(command.GetType(), typeof(TResult) );

            if ( types.TryGetValue(type, out var handlers) )
            {
                foreach (ICommandHandlerResult<TResult> commandHandler in handlers)
                {
                    if (commandHandler.CanHandle(context, command) )
                    {
                        result = commandHandler;

                        return true;
                    }
                }
            }

            result = null;

            return false;
        }
    }
}