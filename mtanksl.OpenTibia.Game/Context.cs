using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenTibia.Game
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

        private Database database;

        public Database Database
        {
            get
            {
                return database ?? (database = new Database(server) );
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

            [DebuggerStepThrough] Promise Next()
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

            [DebuggerStepThrough] PromiseResult<TResult> Next()
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

        private Queue< (GameObject, GameEventArgs) > events;

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddEvent(GameEventArgs e)
        {
            AddEvent(null, e);
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public void AddEvent(GameObject gameObject, GameEventArgs e)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Context) );
            }

            if (events == null)
            {
                events = new Queue< (GameObject, GameEventArgs) >();
            }

            events.Enqueue( (gameObject, e) );
        }

        private Dictionary<IConnection, Message> messages;

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

            if (messages == null)
            {
                messages = new Dictionary<IConnection, Message>();
            }

            Message message;

            if ( !messages.TryGetValue(connection, out message) )
            {
                message = new Message();

                messages.Add(connection, message);
            }

            message.Add(packet);
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

                    if (e.Item1 == null)
                    {
                        foreach (var eventHandler in server.EventHandlers.GetEventHandlers(e.Item2) )
                        {
                            eventHandler.Handle(e.Item2).Catch( (ex) =>
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
                    else
                    {
                        foreach (var eventHandler in server.GameObjectEventHandlers.GetEventHandlers(e.Item1, e.Item2) )
                        {
                            eventHandler.Handle(e.Item2).Catch( (ex) =>
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

            if (messages != null)
            {
                foreach (var pair in messages)
                {
                    IConnection connection = pair.Key;

                    Message message = pair.Value;

                    connection.Send( message.GetBytes(connection.Keys) );
                }

                messages.Clear();
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
                    if (database != null)
                    {
                        database.Dispose();
                    }
                }
            }
        }        
    }
}