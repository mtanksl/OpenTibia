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

        /*
        public void Add<T, TInstance>() where T : Command
        {
            Add<T>( (context, next, command) =>
            {
                var obj = Activator.CreateInstance(typeof(TInstance), new object[] { next } );

                var result = (Promise)typeof(TInstance).GetMethod("Handle").Invoke(obj, new object[] { context, command } );

                return result;
            } );
        }
        */

        public void Add<T>(Func<Context, ContextPromiseDelegate, T, Promise> handle) where T : Command
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

        /*
        public void Add<T, TResult, TInstance>() where T : CommandResult<TResult>
        {
            Add<T, TResult>( (context, next, command) =>
            {
                var obj = Activator.CreateInstance(typeof(TInstance), new object[] { next } );

                var result = (PromiseResult<TResult>)typeof(TInstance).GetMethod("Handle").Invoke(obj, new object[] { context, command } );

                return result;
            } );
        }
        */

        public void Add<T, TResult>(Func<Context, ContextPromiseResultDelegate<TResult>, T, PromiseResult<TResult> > handle) where T : CommandResult<TResult>
        {
            Add(new InlineCommandHandlerResult<T, TResult>(handle) );
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

        public IEnumerable< ICommandHandlerResult<TResult> > Get<TResult>(CommandResult<TResult> command)
        {
            var type = typeof( CommandHandlerResult<,> ).MakeGenericType(command.GetType(), typeof(TResult) );

            if ( types.TryGetValue(type, out var commandHandlers) )
            {
                return commandHandlers.Cast< ICommandHandlerResult<TResult> >();
            }

            return Enumerable.Empty< ICommandHandlerResult<TResult> >();
        }
    }
}