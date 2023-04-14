using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenTibia.Game
{
    public class CommandHandlerCollection
    {
        private Dictionary<Type, List<object> > types = new Dictionary<Type, List<object> >();

        public void Add<T>(Func<Context, Func<Promise>, T, Promise> handle) where T : Command
        {
            Add(new InlineCommandHandler<T>(handle) );
        }

        public void Add<T>(CommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( CommandHandler<> ).MakeGenericType(typeof(T) );
           
            if ( !types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers = new List<object>();

                types.Add(type, commandHandlers);
            }

            commandHandlers.Add(commandHandler);
        }

        public void Add<T, TResult>(Func<Context, Func<PromiseResult<TResult>>, T, PromiseResult<TResult> > handle) where T : CommandResult<TResult>
        {
            Add(new InlineCommandResultHandler<T, TResult>(handle) );
        }

        public void Add<T, TResult>(CommandHandlerResult<T, TResult> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(typeof(T), typeof(TResult) );

            if ( !types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers = new List<object>();

                types.Add(type, commandHandlers);
            }

            commandHandlers.Add(commandHandler);
        }

        public void Remove<T>(CommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( CommandHandler<> ).MakeGenericType(typeof(T) );
           
            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers.Remove(commandHandler);

                if (commandHandlers.Count == 0)
                {
                    types.Remove(type);
                }
            }
        }

        public void Remove<T, TResult>(CommandHandlerResult<T, TResult> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(typeof(T), typeof(TResult) );

            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers.Remove(commandHandler);

                if (commandHandlers.Count == 0)
                {
                    types.Remove(type);
                }
            }
        }

        public IEnumerable<ICommandHandler> Get(Command command)
        {
            var type = typeof( CommandHandler<> ).MakeGenericType(command.GetType() );

            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                return commandHandlers.Cast<ICommandHandler>();
            }

            return Enumerable.Empty<ICommandHandler>();
        }

        public IEnumerable< ICommandResultHandler<TResult> > Get<TResult>(CommandResult<TResult> command)
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(command.GetType(), typeof(TResult) );

            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                return commandHandlers.Cast< ICommandResultHandler<TResult> >();
            }

            return Enumerable.Empty< ICommandResultHandler<TResult> >();
        }
    }
}