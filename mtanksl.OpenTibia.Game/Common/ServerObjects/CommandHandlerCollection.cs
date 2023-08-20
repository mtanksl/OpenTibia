using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class CommandHandlerCollection
    {
        private Dictionary<Type, Dictionary<Guid, object> > types = new Dictionary<Type, Dictionary<Guid, object> >();

        public int Count
        {
            get
            {
                return types.Count;
            }
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid AddCommandHandler<T>(Func<Context, Func<Promise>, T, Promise> handle) where T : Command
        {
            return AddCommandHandler(new InlineCommandHandler<T>(handle) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid AddCommandHandler<T>(ICommandHandler<T> commandHandler) where T : Command
        {
            if (commandHandler.IsDestroyed)
            {
                throw new InvalidOperationException("CommandHandler is destroyed.");
            }

            var type = typeof( ICommandHandler<> ).MakeGenericType(typeof(T) );

            Dictionary<Guid, object> commandHandlers;

            if ( !types.TryGetValue(type, out commandHandlers) )
            {
                commandHandlers = new Dictionary<Guid, object>();

                types.Add(type, commandHandlers);
            }

            commandHandlers.Add(commandHandler.Token, commandHandler);

            return commandHandler.Token;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid AddCommandHandler<TResult, T>(Func<Context, Func<PromiseResult<TResult>>, T, PromiseResult<TResult> > handle) where T : CommandResult<TResult>
        {
            return AddCommandHandler(new InlineCommandResultHandler<TResult, T>(handle) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public Guid AddCommandHandler<TResult, T>(ICommandResultHandler<TResult, T> commandResultHandler) where T : CommandResult<TResult>
        {
            if (commandResultHandler.IsDestroyed)
            {
                throw new InvalidOperationException("CommandHandler is destroyed.");
            }

            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), typeof(T) );

            Dictionary<Guid, object> commandResultHandlers;

            if ( !types.TryGetValue(type, out commandResultHandlers) )
            {
                commandResultHandlers = new Dictionary<Guid, object>();
                
                types.Add(type, commandResultHandlers);
            }

            commandResultHandlers.Add(commandResultHandler.Token, commandResultHandler);

            return commandResultHandler.Token;
        }

        public bool RemoveCommandHandler<T>(Guid token) where T : Command
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(typeof(T) );

            Dictionary<Guid, object> commandHandlers;

            if ( types.TryGetValue(type, out commandHandlers) )
            {
                object obj;

                if (commandHandlers.TryGetValue(token, out obj) )
                {
                    ICommandHandler commandHandler = (ICommandHandler)obj;

                    if ( !commandHandler.IsDestroyed)
                    {
                        commandHandler.IsDestroyed = true;

                        commandHandlers.Remove(token);

                        if (commandHandlers.Count == 0)
                        {
                            types.Remove(type);
                        }

                        return true;
                    }
                }               
            }

            return false;
        }

        public bool RemoveCommandHandler<TResult, T>(Guid token) where T : CommandResult<TResult>
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), typeof(T) );

            Dictionary<Guid, object> commandResultHandlers;

            if ( types.TryGetValue(type, out commandResultHandlers) )
            {
                object obj;

                if (commandResultHandlers.TryGetValue(token, out obj) )
                {
                    ICommandResultHandler<TResult> commandResultHandler = (ICommandResultHandler<TResult>)obj;

                    if ( !commandResultHandler.IsDestroyed)
                    {
                        commandResultHandler.IsDestroyed = true;

                        commandResultHandlers.Remove(token);

                        if (commandResultHandlers.Count == 0)
                        {
                            types.Remove(type);
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public IEnumerable<ICommandHandler> GetCommandHandlers(Command command)
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(command.GetType() );

            Dictionary<Guid, object> commandHandlers;

            if ( types.TryGetValue(type, out commandHandlers) )
            {
                foreach (ICommandHandler commandHandler in commandHandlers.Values.ToList() )
                {
                    if ( !commandHandler.IsDestroyed )
                    {
                        yield return commandHandler;
                    }
                }
            }
        }

        public IEnumerable< ICommandResultHandler<TResult> > GetCommandResultHandlers<TResult>(CommandResult<TResult> command)
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), command.GetType() );

            Dictionary<Guid, object> commandResultHandlers;

            if ( types.TryGetValue(type, out commandResultHandlers) )
            {
                foreach (ICommandResultHandler<TResult> commandResultHandler in commandResultHandlers.Values.ToList() )
                {
                    if ( !commandResultHandler.IsDestroyed )
                    {
                        yield return commandResultHandler;
                    }
                }
            }
        }

        public void Clear()
        {
            types.Clear();
        }
    }
}