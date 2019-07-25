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
        private Dispatcher dispatcher;

        private Scheduler scheduler;

        private List<Listener> listeners = new List<Listener>();

        public Server()
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);

            listeners.Add(new Listener(7171, socket => new LoginConnection(this, socket) ) );

            listeners.Add(new Listener(7172, socket => new GameConnection(this, socket) ) );
        }

        ~Server()
        {
            Dispose(false);
        }

        public Clock Clock { get; set; }

        public Logger Logger { get; set; }

        public ChannelCollection Channels { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public PacketsFactory PacketsFactory { get; set; }

        public Pathfinding Pathfinding { get; set; }

        public ItemFactory ItemFactory { get; set; }

        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public Map Map { get; set; }

        public void Start()
        {
            Clock = new Clock(12, 0);

            Logger = new Logger();

            Channels = new ChannelCollection();

            RuleViolations = new RuleViolationCollection();

            PacketsFactory = new PacketsFactory();

            Pathfinding = new Pathfinding(this);

            using (Logger.Measure("Loading items", true) )
            {
                ItemFactory = new ItemFactory(OtbFile.Load("data/items/items.otb"), DatFile.Load("data/items/tibia.dat"), ItemsFile.Load("data/items/items.xml") );
            }

            using (Logger.Measure("Loading monsters", true) )
            {
                MonsterFactory = new MonsterFactory(MonsterFile.Load("data/monsters") );
            }

            using (Logger.Measure("Loading npcs", true) )
            {
                NpcFactory = new NpcFactory(NpcFile.Load("data/npcs") );
            }

            using (Logger.Measure("Loading map", true) )
            {
                Map = new Map(this, OtbmFile.Load("data/map/pholium3.otbm") );
            }

            QueueForExecution(Clock.Key, Clock.Interval, new GlobalLightCommand() );

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
                using (var context = new CommandContext() )
                {
                    try
                    {
                        command.Execute(this, context);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString() );
                    }

                    context.Flush();
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

            events.Add(key, scheduler.QueueForExecution(new SchedulerEvent(executeIn, () =>
            {
                events.Remove(key);

                using (var context = new CommandContext() )
                {
                    try
                    {
                        command.Execute(this, context);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString() );
                    }

                    context.Flush();
                }                
            } ) ) );
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
            foreach (var listener in listeners)
            {
                listener.Stop();
            }

            CancelQueueForExecution(Clock.Key);

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