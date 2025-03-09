using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenTibia.Game.Common
{
    public class Context : IDisposable
    {
        public static Context Current
        {
            get
            {
                var scope = Scope<Context>.Current;

                if (scope == null)
                {
                    return null;
                }

                return scope.Value;
            }
        }

        public Context(IServer server, Context previousContext)
        {
            this.server = server;

            if (previousContext != null)
            {
                this.data = previousContext.data;
            }
        }

        ~Context()
        {
            Dispose(false);
        }

        private IServer server;

        public IServer Server
        {
            get
            {
                return server;
            }
        }

        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

        public void Post(Action run)
        {
            server.Post(this, run);
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public Promise AddCommand(Command command)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            var commandHandlers = server.CommandHandlers
                .GetCommandHandlers(command)
                .GetEnumerator();

            [DebuggerStepThrough] 
            Promise Next()
            {
                if (commandHandlers.MoveNext() )
                {
                    var commandHandler = commandHandlers.Current;

                    return commandHandler.Handle(Next, command);
                }

                return command.Execute();
            }

            return Next();
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public PromiseResult<TResult> AddCommand<TResult>(CommandResult<TResult> command)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            var commandHandlers = server.CommandHandlers
                .GetCommandResultHandlers(command)
                .GetEnumerator();

            [DebuggerStepThrough]
            PromiseResult<TResult> Next()
            {
                if (commandHandlers.MoveNext() )
                {
                    var commandHandler = commandHandlers.Current;

                    return commandHandler.Handle(Next, command);
                }

                return command.Execute();
            }

            return Next();
        }

        private Queue< (GameObject EventSource, Position Position, GameEventArgs Event) > events;

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddEvent(GameEventArgs e)
        {
            AddEvent(null, null, e);
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddEvent(GameObject eventSource, GameEventArgs e)
        {
            Position position = null;

            switch (eventSource)
            {
                case Item item:

                    switch (item.Root() )
                    {
                        case Tile tile:

                            position = tile.Position;

                            break;

                        case Inventory inventory:

                            position = inventory.Player.Tile?.Position;

                            break;

                        case Safe safe:

                            position = safe.Player.Tile?.Position;

                            break;
                    }

                    break;

                case Creature creature:

                    position = creature.Tile?.Position;

                    break;
            }

            AddEvent(eventSource, position, e);
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddEvent(GameObject eventSource, Position position, GameEventArgs e)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            if (events == null)
            {
                events = new Queue< (GameObject, Position, GameEventArgs) >();
            }

            events.Enqueue( (eventSource, position, e) );
        }

        private Dictionary<IConnection, IMessageCollection> messageCollections;

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddPacket(Player player, IOutgoingPacket packet)
        {
            if (player.Client != null && player.Client.Connection != null)
            {
                AddPacket(player.Client.Connection, packet);
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddPacket(IConnection connection, IOutgoingPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            if (messageCollections == null)
            {
                messageCollections = new Dictionary<IConnection, IMessageCollection>();
            }

            IMessageCollection messageCollection;

            if ( !messageCollections.TryGetValue(connection, out messageCollection) )
            {
                messageCollection = server.MessageCollectionFactory.Create();

                messageCollections.Add(connection, messageCollection);
            }

            messageCollection.Add(packet, server.Features);
        }

        private HashSet<IConnection> connections;

        /// <exception cref="ObjectDisposedException"></exception>

        public void Disconnect(Player player)
        {
            if (player.Client != null && player.Client.Connection != null)
            {
                Disconnect(player.Client.Connection);
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void Disconnect(IConnection connection)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            if (connections == null)
            {
                connections = new HashSet<IConnection>();
            }

            connections.Add(connection);
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void Flush()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            if (events != null)
            {
                while (events.Count > 0)
                {
                    var e = events.Dequeue();

                    foreach (var eventHandler in server.EventHandlers.GetEventHandlers(e.Event) )
                    {
                        eventHandler.Handle(e.Event).Catch( (ex) =>
                        {
                            if (ex is PromiseCanceledException)
                            {
                                //
                            }
                            else
                            {
                                server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                            }
                        } );
                    }

                    if (e.EventSource != null)
                    {
                        foreach (var eventHandler in server.GameObjectEventHandlers.GetEventHandlers(e.EventSource, e.Event) )
                        {
                            eventHandler.Handle(e.Event).Catch( (ex) =>
                            {
                                if (ex is PromiseCanceledException)
                                {
                                    //
                                }
                                else
                                {
                                    server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                                }
                            } );
                        }
                    }

                    if (e.Position != null)
                    {
                        foreach (var eventHandler in server.PositionalEventHandlers.GetEventHandlers(e.Position, e.Event) )
                        {
                            eventHandler.Handle(e.Event).Catch( (ex) =>
                            {
                                if (ex is PromiseCanceledException)
                                {
                                    //
                                }
                                else
                                {
                                    server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                                }
                            } );
                        }
                    }
                }
            }

            if (messageCollections != null)
            {
                foreach (var pair in messageCollections)
                {
                    IConnection connection = pair.Key;

                    IMessageCollection messageCollection = pair.Value;

                    connection.Send(messageCollection);

                    messageCollection.Dispose();
                }

                messageCollections.Clear();
            }

            if (connections != null)
            {
                foreach (var connection in connections)
                {
                    connection.Disconnect();
                }

                connections.Clear();
            }
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if ( !disposed )
            {
                disposed = true;

                if (disposing)
                {

                }
            }
        }        
    }
}