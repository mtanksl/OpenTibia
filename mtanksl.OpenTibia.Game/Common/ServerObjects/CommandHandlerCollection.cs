using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
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
                
        public void Add<T>(ICommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(typeof(T) );
           
            if ( !types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers = new List<object>();

                types.Add(type, commandHandlers);
            }

            commandHandlers.Add(commandHandler);
        }

        public void Add<TResult, T>(Func<Context, Func<PromiseResult<TResult>>, T, PromiseResult<TResult> > handle) where T : CommandResult<TResult>
        {
            Add(new InlineCommandResultHandler<TResult, T>(handle) );
        }

        public void Add<TResult, T>(ICommandResultHandler<TResult, T> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), typeof(T) );

            if ( !types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers = new List<object>();

                types.Add(type, commandHandlers);
            }

            commandHandlers.Add(commandHandler);
        }

        public void Remove<T>(ICommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(typeof(T) );
           
            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                commandHandlers.Remove(commandHandler);

                if (commandHandlers.Count == 0)
                {
                    types.Remove(type);
                }
            }
        }

        public void Remove<TResult, T>(ICommandResultHandler<TResult, T> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), typeof(T) );

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
            var type = typeof( ICommandHandler<> ).MakeGenericType(command.GetType() );

            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                return commandHandlers.Cast<ICommandHandler>();
            }

            return Enumerable.Empty<ICommandHandler>();
        }

        public IEnumerable< ICommandResultHandler<TResult> > Get<TResult>(CommandResult<TResult> command)
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), command.GetType() );

            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                return commandHandlers.Cast< ICommandResultHandler<TResult> >();
            }

            return Enumerable.Empty< ICommandResultHandler<TResult> >();
        }
    }
}