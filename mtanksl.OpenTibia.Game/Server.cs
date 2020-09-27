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
        public Server()
        {
          
        }

        ~Server()
        {
            Dispose(false);
        }

        private Dispatcher dispatcher;

        private Scheduler scheduler;

        private List<Listener> listeners;

        public Clock Clock { get; set; }

        public Logger Logger { get; set; }

        public ChannelCollection Channels { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public GameObjectCollection GameObjects { get; set; }

        public PacketsFactory PacketsFactory { get; set; }

        public ItemFactory ItemFactory { get; set; }

        public PlayerFactory PlayerFactory { get; set; }

        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public IMap Map { get; set; }

        public Pathfinding Pathfinding { get; set; }

        public EventsCollection Events { get; set; }

        public CommandHandlerCollection CommandHandlers { get; set; }

        public ScriptsCollection Scripts { get; set; }

        public void Start()
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);

            listeners = new List<Listener>();

            listeners.Add(new Listener(7171, socket => new LoginConnection(this, socket) ) );

            listeners.Add(new Listener(7172, socket => new GameConnection(this, socket) ) );

            Clock = new Clock(12, 0);

            Logger = new Logger();

            Channels = new ChannelCollection();

            RuleViolations = new RuleViolationCollection();

            GameObjects = new GameObjectCollection(this);

            PacketsFactory = new PacketsFactory();
            
            using (Logger.Measure("Loading items", true) )
            {
                var otb = OtbFile.Load("data/items/items.otb");

                var dat = DatFile.Load("data/items/tibia.dat");

                var items = ItemsFile.Load("data/items/items.xml");

                ItemFactory = new ItemFactory(GameObjects, otb, dat, items);
            }

            using (Logger.Measure("Loading monsters", true) )
            {
                var monsters = MonsterFile.Load("data/monsters");

                MonsterFactory = new MonsterFactory(GameObjects, monsters);
            }

            PlayerFactory = new PlayerFactory(GameObjects);

            using (Logger.Measure("Loading npcs", true) )
            {
                var npcs = NpcFile.Load("data/npcs");

                NpcFactory = new NpcFactory(GameObjects, npcs);
            }

            using (Logger.Measure("Loading map", true) )
            {
                var otbm = OtbmFile.Load("data/map/pholium3.otbm");

                Map = new Map(ItemFactory, otbm);
            }

            Pathfinding = new Pathfinding(Map);

            Events = new EventsCollection();

            CommandHandlers = new CommandHandlerCollection(this);

            Scripts = new ScriptsCollection(this);

            using (Logger.Measure("Loading scripts", true) )
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

        public void QueueForExecution(Command command)
        {
            dispatcher.QueueForExecution( () =>
            {
                using (var context = new Context(this) )
                {
                    try
                    {
                        command.Execute(context);

                        context.Flush();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString() );
                    }
                }
            } );
        }

        private Dictionary<string, SchedulerEvent> events = new Dictionary<string, SchedulerEvent>();

        public void QueueForExecution(string key, int executeIn, Command command)
        {
            SchedulerEvent schedulerEvent;

            if ( events.TryGetValue(key, out schedulerEvent) )
            {
                events.Remove(key);

                schedulerEvent.Cancel();
            }

            events.Add(key, scheduler.QueueForExecution(executeIn, () =>
            {
                events.Remove(key);

                using (var context = new Context(this) )
                {
                    try
                    {
                        command.Execute(context);

                        context.Flush();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString() );
                    }
                }                
            } ) );
        }

        public bool CancelQueueForExecution(string key)
        {
            SchedulerEvent schedulerEvent;

            if ( events.TryGetValue(key, out schedulerEvent) )
            {
                events.Remove(key);

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