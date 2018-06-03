using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Connections;
using OpenTibia.Mvc;
using OpenTibia.Network.Sockets;
using OpenTibia.Threading;
using OpenTibia.Web;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Server : IDisposable
    {
        private Dispatcher dispatcher;

        private Scheduler scheduler;

        private Listener loginListener;

        private Listener gameListener;

        public Server()
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);
            
            loginListener = new Listener(7171, socket => new LoginConnection(this, 7171, socket) );

            gameListener = new Listener(7172, socket => new GameConnection(this, 7172, socket) );
        }

        ~Server()
        {
            Dispose(false);
        }

        public Logger Logger { get; set; }

        public ChannelCollection Channels { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public ControllerMetadataFactory ControllerBaseMetadataFactory { get; set; }

        public ItemFactory ItemFactory { get; set; }
        
        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public Map Map { get; set; }
        
        public void Start()
        {
            Logger = new Logger();

            Channels = new ChannelCollection();

            RuleViolations = new RuleViolationCollection();

            ControllerBaseMetadataFactory = new ControllerMetadataFactory();

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

            dispatcher.Start();

            scheduler.Start();

            loginListener.Start();

            gameListener.Start();

            Logger.WriteLine("Server online");
        }

        private Dictionary<string, SchedulerEvent> schedulerEvents = new Dictionary<string, SchedulerEvent>();

        public void QueueForExecution(Context context, Command command, Action callback)
        {
            dispatcher.QueueForExecution( () =>
            {
                try
                {
                    command.Execute(context);

                        context.Response.Flush();

                    if (callback != null)
                    {
                        callback();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.ToString() );
                }                
            } );
        }

        public void QueueForExecution(string key, int delay, Context context, Command command, Action callback)
        {
            SchedulerEvent schedulerEvent;

            if ( schedulerEvents.TryGetValue(key, out schedulerEvent) )
            {
                schedulerEvents.Remove(key);

                schedulerEvent.Cancel();
            }

            schedulerEvent = scheduler.QueueForExecution(delay, () =>
            {
                schedulerEvents.Remove(key);

                try
                {
                    command.Execute(context);

                        context.Response.Flush();

                    if (callback != null)
                    {
                        callback();
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.ToString() );
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
            gameListener.Stop();

            loginListener.Stop();

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
                    loginListener.Dispose();

                    gameListener.Dispose();
                }
            }
        }
    }
}