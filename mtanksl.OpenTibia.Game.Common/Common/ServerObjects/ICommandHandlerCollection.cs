using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ICommandHandlerCollection
    {
        int Count { get; }

        Guid AddCommandHandler<T>(Func<Context, Func<Promise>, T, Promise> handle) where T : Command;

        Guid AddCommandHandler<T>(ICommandHandler<T> commandHandler) where T : Command;

        Guid AddCommandHandler<TResult, T>(Func<Context, Func<PromiseResult<TResult>>, T, PromiseResult<TResult>> handle) where T : CommandResult<TResult>;

        Guid AddCommandHandler<TResult, T>(ICommandResultHandler<TResult, T> commandResultHandler) where T : CommandResult<TResult>;

        bool RemoveCommandHandler<T>(Guid token) where T : Command;

        bool RemoveCommandHandler<TResult, T>(Guid token) where T : CommandResult<TResult>;

        IEnumerable<ICommandHandler> GetCommandHandlers(Command command);

        IEnumerable<ICommandResultHandler<TResult>> GetCommandResultHandlers<TResult>(CommandResult<TResult> command);

        void Clear();
    }
}