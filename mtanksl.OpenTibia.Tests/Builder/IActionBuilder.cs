using OpenTibia.Common.Objects;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Tests
{
    public interface IActionBuilder
    {
        IConnection Connection { get; }

        IActionBuilder Execute<TCommand>(TCommand command) where TCommand : Command;

        IActionBuilder Execute<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler) where TCommand : Command;

        IActionBuilder Execute<TEvent>(TEvent e, IEventHandler<TEvent> eventHandler) where TEvent : GameEventArgs;

        IActionBuilder ExpectSuccess(bool success = true);

        IActionBuilder Observe(Action<IObserveBuilder> options);

        IActionBuilder Observe(Player player, Action<IObserveBuilder> options);

        IActionBuilder Observe(IConnection connection, Action<IObserveBuilder> options);

        void Run();
    }
}