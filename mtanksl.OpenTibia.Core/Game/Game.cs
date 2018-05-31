using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Threading;
using System;

namespace OpenTibia
{
    public class Game : IDisposable
    {
        private static Game current;

        public static Game Current
        {
            get
            {
                return current;
            }
        }
        
        public Game()
        {
            current = this;
            
            Log = new Logger();

            Chats = new ChatCollection();

            RuleViolations = new RuleViolationCollection();

            Dispatcher = new Dispatcher();

            Dispatcher.Complete += (s, e) =>
            {
                DispatcherContext context = DispatcherContext.Current;

                if (context != null)
                {
                    context.GetItem<MessageCollection>("Messages", () => new MessageCollection() ).Flush();
                }
            };

            Scheduler = new Scheduler(Dispatcher);

            LoginListener = new Listener(7171, new TibiaLoginClientFactory() );

            GameListener = new Listener(7172, new TibiaGameClientFactory() );
        }

        ~Game()
        {
            Dispose(false);
        }

        public Logger Log { get; set; }

        public ChatCollection Chats { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public ItemFactory ItemFactory { get; set; }

        public MonsterFactory MonsterFactory { get; set; }

        public NpcFactory NpcFactory { get; set; }

        public Map Map { get; set; }

        public IncomingPacketFactory IncomingPacketFactory { get; set; }

        public OutgoingPacketFactory OutgoingPacketFactory { get; set; }

        public EventBus EventBus { get; set; }

        public Dispatcher Dispatcher { get; set; }

        public Scheduler Scheduler { get; set; }
        
        public Listener LoginListener { get; set; }

        public Listener GameListener { get; set; }

        public byte Level = 250;

        private SchedulerEvent lightSchedulerEvent;

        private SchedulerEvent pingSchedulerEvent;

        public void Start()
        {
            using ( Log.Measure("Loading items", true) )
            {
                OtbFile otbFile = OtbFile.Load("data/items/items.otb");

                DatFile datFile = DatFile.Load("data/items/tibia.dat");

                ItemsFile itemsFile = ItemsFile.Load("data/items/items.xml");

                ItemFactory = new ItemFactory(otbFile, datFile, itemsFile);
            }

            using ( Log.Measure("Loading monsters", true) )
            {
                MonsterFile monsterFile = MonsterFile.Load("data/monsters");

                MonsterFactory = new MonsterFactory(monsterFile);
            }
                        
            using ( Log.Measure("Loading npcs", true) )
            {
                NpcFile npcFile = NpcFile.Load("data/npcs");

                NpcFactory = new NpcFactory(npcFile);
            }

            using ( Log.Measure("Loading otbm", true) )
            {
                //OtbmFile otbmFile = OtbmFile.Load("data/map/pholium3.otbm"

                //Map = new Map(otbmFile);

                Map = new Map("data/map/pholium3.otbm");
            }

            IncomingPacketFactory = new IncomingPacketFactory();

            OutgoingPacketFactory = new OutgoingPacketFactory();

            EventBus = new EventBus();

            EventBus.Subscribe<CreatureMoveEvent>(EventHandlers.NotifyCreatureMove);

            EventBus.Subscribe<CreatureMoveEvent>(EventHandlers.WalkTile);

                EventBus.Subscribe<TileReplaceEvent>(EventHandlers.NotifyTileReplace);
            
            EventBus.Subscribe<CreatureMoveEvent>(EventHandlers.WalkFire);

                EventBus.Subscribe<MagicEffectEvent>(EventHandlers.NotifyMagicEffect);

                EventBus.Subscribe<CreatureChangeHealthEvent>(EventHandlers.NotifyCreatureChangeHealth);
                
                EventBus.Subscribe<CreatureRemoveEvent>(EventHandlers.NotifyCreatureRemove);

            EventBus.Subscribe<CreatureTurnEvent>(EventHandlers.NotifyCreatureTurn);

            EventBus.Subscribe<CreatureChangeOutfitEvent>(EventHandlers.NotifyCreatureChangeOutfit);
            
            Dispatcher.Start();

            Scheduler.Start();
            
            int hour = 12;

            int minute = 0;

            double minutes = hour * 60 + minute;

            int interval = 10000;        // 10 seconds in real life...

            int delta = interval / 2500; // ...is 4 minutes in the game
            
            Action callback1 = null; callback1 = () =>
            {
                lightSchedulerEvent = Scheduler.QueueForExecution(interval, () =>
                {
                    minute += delta;

                    if (minute == 60)
                    {
                        minute = 0;

                        hour += 1;

                        if (hour == 24)
                        {
                            hour = 0;
                        }
                    }

                    minutes += delta;

                    if (minutes == 1440)
                    {
                        minutes = 0;
                    }
                    
                    if (minutes <= 720) // Sunrise
                    {
                        // level vai de 40 a 250
                        // minutes vai de 0 a 720

                        Level = (byte)( ( (minutes / 720) * 210) + 40);
                    }
                    else // Sunset
                    {
                        // level vai de 250 a 40
                        // minutes vai de 720 a 1440

                        Level = (byte)( ( ( (minutes - 720) / 720) * -210) + 250);
                    }

                    foreach (var player in Map.GetPlayers() )
                    {
                        player.Client.Response.Write(new EnvironmentLightOutgoingPacket(new Light(Level, 215) ) );
                    }

                    callback1();
                } );
            };

            callback1();
            
            Action callback2 = null; callback2 = () =>
            {
                pingSchedulerEvent = Scheduler.QueueForExecution(5000, () =>
                {
                    foreach (var player in Map.GetPlayers() )
                    {
                        player.Client.PingRequest = DateTime.Now;

                        player.Client.Response.Write(new PingOutgoingPacket() );
                    }

                    callback2();
                } );
            };

            callback2();

            LoginListener.Start();

            GameListener.Start();
        }

        public void Stop()
        {
            GameListener.Stop();

            LoginListener.Stop();

            pingSchedulerEvent.Cancel();

            lightSchedulerEvent.Cancel();

            Scheduler.Stop();

            Dispatcher.Stop();
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
                    LoginListener.Dispose();

                    GameListener.Dispose();
                }                

                current = null;
            }
        }
    }
}