using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Sockets;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Server : IDisposable
    {
        public Server(int loginServerPort, int gameServerPort)
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);

            listeners = new List<Listener>()
            {
                new Listener(loginServerPort, socket => new LoginConnection(this, socket) ),

                new Listener(gameServerPort, socket => new GameConnection(this, socket) )
            };

            PacketsFactory = new PacketsFactory();

            Logger = new Logger(new ConsoleLoggerProvider(), LogLevel.Debug);

            Clock = new Clock(12, 00);

            Randomization = new Randomization();

            Channels = new ChannelCollection();

            RuleViolations = new RuleViolationCollection();

            Lockers = new LockerCollection();

            Components = new ComponentCollection(this);

            GameObjects = new GameObjectCollection();

            CommandHandlers = new CommandHandlerCollection();

            EventHandlers = new EventHandlerCollection();

            Scripts = new ScriptsCollection(this);
        }

        ~Server()
        {
            Dispose(false);
        }

        private Dispatcher dispatcher;

        private Scheduler scheduler;

        private List<Listener> listeners;

        public PacketsFactory PacketsFactory { get; set; }

        public Logger Logger { get; set; }

        public Clock Clock { get; set; }

        public Randomization Randomization { get; set; }

        public ChannelCollection Channels { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public LockerCollection Lockers { get; set; }

        public ComponentCollection Components { get; set; }

        public GameObjectCollection GameObjects { get; set; }

        public CommandHandlerCollection CommandHandlers { get; set; }

        public EventHandlerCollection EventHandlers { get; set; }

        public ScriptsCollection Scripts { get; set; }


        public ItemFactory ItemFactory { get; set; }

        public PlayerFactory PlayerFactory { get; set; }

        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public IMap Map { get; set; }

        public Pathfinding Pathfinding { get; set; }     

        public void Start()
        {
            Logger.WriteLine("An open Tibia server developed by mtanksl");
            Logger.WriteLine("Source code: https://github.com/mtanksl/OpenTibia");
            Logger.WriteLine();

            using (Logger.Measure("Loading items") )
            {
                ItemFactory = new ItemFactory(this, OtbFile.Load("data/items/items.otb"), DatFile.Load("data/items/tibia.dat"), ItemsFile.Load("data/items/items.xml") );
            }

            PlayerFactory = new PlayerFactory(this);

            using (Logger.Measure("Loading monsters") )
            {
                MonsterFactory = new MonsterFactory(this, MonsterFile.Load("data/monsters") );
            }

            using (Logger.Measure("Loading npcs") )
            {
                NpcFactory = new NpcFactory(this, NpcFile.Load("data/npcs") );
            }

            using (Logger.Measure("Loading map") )
            {
                Map = new Map(ItemFactory, OtbmFile.Load("data/map/map.otbm") );
            }

            Pathfinding = new Pathfinding(Map);

            dispatcher.Start();

            scheduler.Start();

            QueueForExecution( () =>
            {
                Scripts.Start();

                return Promise.Completed;

            } ).Wait();

            foreach (var listener in listeners)
            {
                listener.Start();
            }

            Logger.WriteLine("Server online");
        }

        private Dictionary<string, SchedulerEvent> schedulerEvents = new Dictionary<string, SchedulerEvent>();

        public Promise QueueForExecution(Func<Promise> run)
        {
            return Promise.Run( (resolve, reject) =>
            {
                Context previousContext = Context.Current;

                DispatcherEvent dispatcherEvent = new DispatcherEvent( () =>
                {
                    try
                    {
                        using (var context = new Context(this, previousContext) )
                        {
                            using (var scope = new Scope<Context>(context) )
                            {
                                run().Then(resolve).Catch( (ex) =>
                                {
                                    if (ex is PromiseCanceledException)
                                    {
                                        //
                                    }
                                    else
                                    {
                                        Logger.WriteLine(ex.ToString(), LogLevel.Error);
                                    }

                                    reject(ex);
                                } );

                                context.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString(), LogLevel.Error);
                    }
                } );

                dispatcherEvent.Canceled += (sender, e) =>
                {
                    Exception ex = new PromiseCanceledException();

                    reject(ex);
                };

                dispatcher.QueueForExecution(dispatcherEvent);
            } );
        }

        public Promise QueueForExecution(string key, TimeSpan executeIn, Func<Promise> run)
        {
            return Promise.Run( (resolve, reject) =>
            {
                SchedulerEvent schedulerEvent;

                if ( schedulerEvents.TryGetValue(key, out schedulerEvent) )
                {
                    schedulerEvents.Remove(key);

                    schedulerEvent.Cancel();
                }

                Context previousContext = Context.Current;

                schedulerEvent = new SchedulerEvent(executeIn, () =>
                {
                    schedulerEvents.Remove(key);

                    try
                    {
                        using (var context = new Context(this, previousContext) )
                        {
                            using (var scope = new Scope<Context>(context) )
                            {
                                run().Then(resolve).Catch( (ex) =>
                                {
                                    if (ex is PromiseCanceledException)
                                    {
                                        //
                                    }
                                    else
                                    {
                                        Logger.WriteLine(ex.ToString(), LogLevel.Error);
                                    }

                                    reject(ex);
                                } );

                                context.Flush();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString(), LogLevel.Error);
                    }
                } );

                schedulerEvent.Canceled += (sender, e) =>
                {
                    Exception ex = new PromiseCanceledException();

                    reject(ex);
                };

                schedulerEvents.Add(key, schedulerEvent);

                scheduler.QueueForExecution(schedulerEvent);
            } );
        }

        public bool CancelQueueForExecution(string key)
        {
            SchedulerEvent schedulerEvent;

            if ( schedulerEvents.TryGetValue(key, out schedulerEvent) )
            {
                schedulerEvents.Remove(key);

                schedulerEvent.Cancel();

                return true;
            }

            return false;
        }

        public void KickAll()
        {
            QueueForExecution( () =>
            {
                Context context = Context.Current;

                List<Promise> promises = new List<Promise>();

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    promises.Add(context.AddCommand(new PlayerDestroyCommand(observer) ) );
                }

                return Promise.WhenAll(promises.ToArray() );

            } ).Wait();
        }

        public void Stop()
        {
            QueueForExecution( () =>
            {
                Scripts.Stop();

                return Promise.Completed;

            } ).Wait();

            foreach (var listener in listeners)
            {
                listener.Stop();
            }

            scheduler.Stop();

            dispatcher.Stop();

            Logger.WriteLine("Server offline");
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    foreach (var listener in listeners)
                    {
                        listener.Dispose();
                    }
                }
            }
        }
    }
}