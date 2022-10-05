using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Network.Sockets;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Server : IDisposable
    {
        private int loginServerPort;
        
        private int gameServerPort;

        public Server(int loginServerPort, int gameServerPort)
        {
            this.loginServerPort = loginServerPort;

            this.gameServerPort = gameServerPort;
        }

        ~Server()
        {
            Dispose(false);
        }

        public Dispatcher dispatcher;

        private Scheduler scheduler;

        private List<Listener> listeners;

        public PacketsFactory PacketsFactory { get; set; }

        public Logger Logger { get; set; }

        public Clock Clock { get; set; }

        public ChannelCollection Channels { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public GameObjectCollection GameObjects { get; set; }

        public ItemFactory ItemFactory { get; set; }

        public PlayerFactory PlayerFactory { get; set; }

        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public IMap Map { get; set; }

        public Pathfinding Pathfinding { get; set; }

        public CommandHandlerCollection CommandHandlers { get; set; }

        public EventHandlerCollection EventHandlers { get; set; }

        public ScriptsCollection Scripts { get; set; }

        public void Start()
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);

            listeners = new List<Listener>();

            listeners.Add(new Listener(loginServerPort, socket => new LoginConnection(this, socket) ) );

            listeners.Add(new Listener(gameServerPort, socket => new GameConnection(this, socket) ) );

            PacketsFactory = new PacketsFactory();

            Logger = new Logger();

            Clock = new Clock(12, 0);

            Channels = new ChannelCollection();

            RuleViolations = new RuleViolationCollection();

            GameObjects = new GameObjectCollection(this);
                        
            using (Logger.Measure("Loading items") )
            {
                var otb = OtbFile.Load("data/items/items.otb");

                var dat = DatFile.Load("data/items/tibia.dat");

                var items = ItemsFile.Load("data/items/items.xml");

                ItemFactory = new ItemFactory(GameObjects, otb, dat, items);
            }

            using (Logger.Measure("Loading monsters") )
            {
                var monsters = MonsterFile.Load("data/monsters");

                MonsterFactory = new MonsterFactory(GameObjects, monsters);
            }

            PlayerFactory = new PlayerFactory(GameObjects);

            using (Logger.Measure("Loading npcs") )
            {
                var npcs = NpcFile.Load("data/npcs");

                NpcFactory = new NpcFactory(GameObjects, npcs);
            }

            using (Logger.Measure("Loading map") )
            {
                var otbm = OtbmFile.Load("data/map/pholium3.otbm");

                Map = new Map(ItemFactory, otbm);
            }

            Pathfinding = new Pathfinding(Map);

            CommandHandlers = new CommandHandlerCollection();

            EventHandlers = new EventHandlerCollection();

            Scripts = new ScriptsCollection(this);

            using (Logger.Measure("Loading scripts") )
            {
                Scripts.Start();
            }

            dispatcher.Start();

            scheduler.Start();

            foreach (var listener in listeners)
            {
                listener.Start();
            }

            Logger.WriteLine("Server online");
        }

        private Dictionary<string, SchedulerEvent> schedulerEvents = new Dictionary<string, SchedulerEvent>();

        public void QueueForExecution(Action<Context> callback)
        {
            dispatcher.QueueForExecution( () =>
            {
                try
                {
                    using (var context = new Context(this) )
                    {
                        using (var scope = new Scope<Context>(context) )
                        {
                            callback(context);
                        }

                        context.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }
            } );
        }

        public void QueueForExecution(string key, int executeInMilliseconds, Action<Context> callback)
        {
            SchedulerEvent schedulerEvent;

            if ( schedulerEvents.TryGetValue(key, out schedulerEvent) )
            {
                schedulerEvents.Remove(key);

                schedulerEvent.Cancel();
            }

            schedulerEvent = scheduler.QueueForExecution(executeInMilliseconds, () =>
            {
                schedulerEvents.Remove(key);

                try
                {
                    using (var context = new Context(this) )
                    {
                        using (var scope = new Scope<Context>(context) )
                        {
                            callback(context);
                        }

                        context.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }
            } );

            schedulerEvents.Add(key, schedulerEvent);
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

        public void Stop()
        {
            Scripts.Stop();

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