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
using System;

namespace OpenTibia.Game
{
    public class Server : IDisposable
    {
        public Server()
        {
            CreatureCollection = new CreatureCollection();

            CommandBus = new CommandBus(this);

            Dispatcher = new Dispatcher();

            Scheduler = new Scheduler(Dispatcher);

            ControllerBaseMetadataFactory = new ControllerMetadataFactory();

            LoginListener = new Listener(7171, socket => new LoginConnection(this, 7171, socket) );

            GameListener = new Listener(7172, socket => new GameConnection(this, 7172, socket) );
        }

        ~Server()
        {
            Dispose(false);
        }
        
        public ItemFactory ItemFactory { get; set; }
        
        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public Map Map { get; set; }

        public CreatureCollection CreatureCollection { get; set; }

        public CommandBus CommandBus { get; set; }
        
        public Dispatcher Dispatcher { get; set; }

        public Scheduler Scheduler { get; set; }

        public ControllerMetadataFactory ControllerBaseMetadataFactory { get; set; }

        public Listener LoginListener { get; set; }

        public Listener GameListener { get; set; }
        
        public void Start()
        {
            OtbFile otbFile = OtbFile.Load("data/items/items.otb");

            DatFile datFile = DatFile.Load("data/items/tibia.dat");

            ItemsFile itemsFile = ItemsFile.Load("data/items/items.xml");

            ItemFactory = new ItemFactory(otbFile, datFile, itemsFile);

            MonsterFile monsterFile = MonsterFile.Load("data/monsters");

            MonsterFactory = new MonsterFactory(monsterFile);

            NpcFile npcFile = NpcFile.Load("data/npcs");

            NpcFactory = new NpcFactory(npcFile);

            OtbmFile otbmFile = OtbmFile.Load("data/map/pholium3.otbm");

            Map = new Map(ItemFactory, otbmFile);

            Dispatcher.Start();

            Scheduler.Start();

            LoginListener.Start();

            GameListener.Start();
        }

        public void Stop()
        {
            GameListener.Stop();

            LoginListener.Stop();

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
            }
        }
    }
}