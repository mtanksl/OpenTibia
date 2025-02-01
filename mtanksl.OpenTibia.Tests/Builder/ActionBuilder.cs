using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Objects;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;

namespace OpenTibia.Tests
{
    public class ActionBuilder : IActionBuilder
    {
        private IServer server;

        private Action<IActionBuilder> options;

        public ActionBuilder(IServer server, IConnection connection, Action<IActionBuilder> options)
        {
            this.server = server;

            this.options = options;

            this.connection = connection;
        }

        private IConnection connection;

        public IConnection Connection
        {
            get
            {
                return connection;
            }
        }

        private Command command;

        private ICommandHandler commandHandler;

        private GameEventArgs e;

        private IEventHandler eventHandler;

        public IActionBuilder Execute<TCommand>(TCommand command) where TCommand : Command
        {
            this.command = command;

            return this;
        }

        public IActionBuilder Execute<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler) where TCommand : Command
        {
            this.command = command;

            this.commandHandler = commandHandler;

            return this;
        }

        public IActionBuilder Execute<TEvent>(TEvent e, IEventHandler<TEvent> eventHandler) where TEvent : GameEventArgs
        {
            this.e = e;

            this.eventHandler = eventHandler;

            return this;
        }

        private bool success;

        public IActionBuilder ExpectSuccess(bool success = true)
        {
            this.success = success;

            return this;
        }

        private List<ObserveBuilder> observeBuilders = new List<ObserveBuilder>();

        public IActionBuilder Observe(Action<IObserveBuilder> options) 
        {
            return Observe(connection, options);
        }

        public IActionBuilder Observe(Player player, Action<IObserveBuilder> options)
        {
            return Observe(player.Client.Connection, options);
        }

        public IActionBuilder Observe(IConnection connection, Action<IObserveBuilder> options)
        {
            var observeBuilder = new ObserveBuilder(server, connection, options);

            this.observeBuilders.Add(observeBuilder);

            return this;
        }

        public void Run()
        {
            server.QueueForExecution( () =>
            {
                options(this);

                return Promise.Completed;

            } ).Wait();

            foreach (var observeBuilder in observeBuilders)
            {
                observeBuilder.InternalRunBegin();
            }

            Promise promise;

            if (commandHandler != null)
            {
                promise = server.QueueForExecution( () =>
                {
                    return commandHandler.Handle( () => command.Execute(), command);
                } );
            }
            else if (eventHandler != null)
            {
                promise = server.QueueForExecution( () =>
                {
                    return eventHandler.Handle(e);
                } );
            }
            else if (command != null)
            {
                promise = server.QueueForExecution( () =>
                {
                    return command.Execute();
                } );
            }
            else
            {
                throw new NotImplementedException();
            }

            if (success)
            {
                promise.Wait();
            }
            else
            {
                Assert.ThrowsException<PromiseCanceledException>( () => promise.Wait(), "Expected failure with PromiseCanceledException.");
            }

            foreach (var observeBuilder in observeBuilders)
            {
                observeBuilder.InternalRunEnd();
            }
        }
    }
}