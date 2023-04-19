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

        public Guid Add<T>(Func<Context, Func<Promise>, T, Promise> handle) where T : Command
        {
            return Add(new InlineCommandHandler<T>(handle) );
        }
                
        public Guid Add<T>(ICommandHandler<T> commandHandler) where T : Command
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(typeof(T) );

            Dictionary<Guid, object> commandHandlers;

            if ( !types.TryGetValue(type, out commandHandlers) )
            {
                commandHandlers = new Dictionary<Guid, object>();

                types.Add(type, commandHandlers);
            }

            commandHandler.Canceled = false;

            commandHandlers.Add(commandHandler.Token, commandHandler);

            return commandHandler.Token;
        }

        public Guid Add<TResult, T>(Func<Context, Func<PromiseResult<TResult>>, T, PromiseResult<TResult> > handle) where T : CommandResult<TResult>
        {
            return Add(new InlineCommandResultHandler<TResult, T>(handle) );
        }

        public Guid Add<TResult, T>(ICommandResultHandler<TResult, T> commandHandler) where T : CommandResult<TResult>
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), typeof(T) );

            Dictionary<Guid, object> commandHandlers;

            if ( !types.TryGetValue(type, out commandHandlers) )
            {
                commandHandlers = new Dictionary<Guid, object>();
                
                types.Add(type, commandHandlers);
            }

            commandHandler.Canceled = false;

            commandHandlers.Add(commandHandler.Token, commandHandler);

            return commandHandler.Token;
        }

        public void Remove<T>(Guid token) where T : Command
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(typeof(T) );

            Dictionary<Guid, object> commandHandlers;

            if ( types.TryGetValue(type, out commandHandlers) )
            {
                object commandHandler;

                if (commandHandlers.TryGetValue(token, out commandHandler) )
                {
                    ( (ICommandHandler)commandHandler ).Canceled = true;

                    commandHandlers.Remove(token);

                    if (commandHandlers.Count == 0)
                    {
                        types.Remove(type);
                    }
                }               
            }
        }

        public void Remove<TResult, T>(Guid token) where T : CommandResult<TResult>
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), typeof(T) );

            Dictionary<Guid, object> commandHandlers;

            if ( types.TryGetValue(type, out commandHandlers) )
            {
                object commandHandler;

                if (commandHandlers.TryGetValue(token, out commandHandler) )
                {
                    ( (ICommandResultHandler<TResult>)commandHandler ).Canceled = true;

                    commandHandlers.Remove(token);

                    if (commandHandlers.Count == 0)
                    {
                        types.Remove(type);
                    }
                }
            }
        }

        public IEnumerable<ICommandHandler> GetCommandHandlers(Command command)
        {
            var type = typeof( ICommandHandler<> ).MakeGenericType(command.GetType() );

            Dictionary<Guid, object> commandHandlers;

            if ( types.TryGetValue(type, out commandHandlers) )
            {
                foreach (ICommandHandler commandHandler in commandHandlers.Values.ToList() )
                {
                    if ( !commandHandler.Canceled )
                    {
                        yield return commandHandler;
                    }
                }
            }
        }

        public IEnumerable< ICommandResultHandler<TResult> > GetCommandResultHandlers<TResult>(CommandResult<TResult> command)
        {
            var type = typeof(ICommandResultHandler<,> ).MakeGenericType(typeof(TResult), command.GetType() );

            Dictionary<Guid, object> commandHandlers;

            if ( types.TryGetValue(type, out commandHandlers) )
            {
                foreach (ICommandResultHandler<TResult> commandHandler in commandHandlers.Values.ToList() )
                {
                    if ( !commandHandler.Canceled )
                    {
                        yield return commandHandler;
                    }
                }
            }
        }
    }
}