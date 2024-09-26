using OpenTibia.Common;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Data.MsSql.Contexts;
using OpenTibia.Data.MySql.Contexts;
using OpenTibia.Data.Sqlite.Contexts;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.FileFormats.Xml.Houses;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.FileFormats.Xml.Spawns;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Sockets;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using House = OpenTibia.Common.Objects.House;
using Item = OpenTibia.Common.Objects.Item;
using Tile = OpenTibia.Common.Objects.Tile;

namespace OpenTibia.Game.Common
{
    public class Server : IServer
    {
        public Server()
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);

            ClientFactory = new ClientFactory(this);

            DatabaseFactory = new DatabaseFactory(this, builder =>
            {
                switch (Config.DatabaseType)
                {
                    case "sqlite":

                        return new SqliteContext("Data Source=" + PathResolver.GetFullPath(Config.DatabaseSource), builder.Options);

                    case "mysql":

                        return new MySqlContext("Server=" + Config.DatabaseHost + ";Port=" + Config.DatabasePort + ";Database=" + Config.DatabaseName + ";User=" + Config.DatabaseUser + ";Password=" + Config.DatabasePassword + ";", builder.Options);

                    case "mssql":

                        return new MsSqlContext("Server=" + Config.DatabaseHost + ";Database=" + Config.DatabaseName + ";User Id=" + Config.DatabaseUser + ";Password=" + Config.DatabasePassword + ";TrustServerCertificate=True;", builder.Options);

                    case "memory":

                        return new InMemoryContext(builder.Options);

                    default:

                        throw new NotImplementedException("File config.lua parameter server.database.type must be sqlite, mysql, mssql or memory.");
                }
            } );

            Statistics = new ServerStatistics();

            Logger = new Logger(new ConsoleLoggerProvider(), LogLevel.Debug);

            PathResolver = new PathResolver();

            Randomization = new Randomization();

            Clock = new Clock(12, 00);

            RateLimiting = new RateLimiting(this);

            WaitingList = new WaitingList(this);

            Channels = new ChannelCollection();

            RuleViolations = new RuleViolationCollection();

            Guilds = new GuildCollection();

            Parties = new PartyCollection();

            Tradings = new TradingCollection();

            NpcTradings = new NpcTradingCollection();

            Combats = new CombatCollection();

            GameObjectPool = new GameObjectPool();

            GameObjects = new GameObjectCollection();

            GameObjectComponents = new GameObjectComponentCollection();

            GameObjectEventHandlers = new GameObjectEventHandlerCollection();

            CommandHandlers = new CommandHandlerCollection();

            EventHandlers = new EventHandlerCollection();

            LuaScripts = new LuaScriptCollection(this);

            PluginLoader = new PluginLoader(this);

            Values = new Values(this);

            Config = new Config(this);

            Quests = new QuestCollection(this);

            Outfits = new OutfitCollection(this);

            Plugins = new PluginCollection(this);

            ItemFactory = new ItemFactory(this);

            PlayerFactory = new PlayerFactory(this);

            MonsterFactory = new MonsterFactory(this);

            NpcFactory = new NpcFactory(this);

            Map = new Map(this);

            Spawns = new SpawnCollection(this);

            Pathfinding = new Pathfinding(Map);

            Scripts = new ScriptCollection(this);

            GameObjectScripts = new GameObjectScriptCollection(this);
        }

        ~Server()
        {
            Dispose(false);
        }

        public string ServerName
        {
            get
            {
                return "MTOTS";
            }
        }

        public string ServerVersion
        {
            get
            {
                return "1.3";
            }
        }

        public string ClientVersion
        {
            get
            {
                return "8.60";
            }
        }

        private Dispatcher dispatcher;

        private Scheduler scheduler;

        private Listener loginServer;

        private Listener gameServer;

        private Listener infoServer;

        public ServerStatus Status { get; private set; }

        public IClientFactory ClientFactory { get; set; }

        public IDatabaseFactory DatabaseFactory { get; set; }

        public IServerStatistics Statistics { get; set; }

        public ILogger Logger { get; set; }

        public IPathResolver PathResolver { get; set; }

        public IRandomization Randomization { get; set; }

        public Clock Clock { get; set; }

        public IRateLimiting RateLimiting { get; set; }

        public IWaitingList WaitingList { get; set; }

        public IChannelCollection Channels { get; set; }

        public IRuleViolationCollection RuleViolations { get; set; }

        public IGuildCollection Guilds { get; set; }

        public IPartyCollection Parties { get; set; }

        public ITradingCollection Tradings { get; set; }

        public INpcTradingCollection NpcTradings { get; set; }

        public ICombatCollection Combats { get; set; }

        public IGameObjectPool GameObjectPool { get; set; }

        public IGameObjectCollection GameObjects { get; set; }

        public IGameObjectComponentCollection GameObjectComponents { get; set; }

        public IGameObjectEventHandlerCollection GameObjectEventHandlers { get; set; }

        public ICommandHandlerCollection CommandHandlers { get; set; }

        public IEventHandlerCollection EventHandlers { get; set; }

        public ILuaScriptCollection LuaScripts { get; set; }

        public IPluginLoader PluginLoader { get; set; }

        public IValues Values { get; set; }

        public IConfig Config { get; set; }

        public IQuestCollection Quests { get; set; }

        public IOutfitCollection Outfits { get; set; }

        public IPluginCollection Plugins { get; set; }

        public IScriptCollection Scripts { get; set; }

        public IGameObjectScriptCollection GameObjectScripts { get; set; }

        public IItemFactory ItemFactory { get; set; }

        public IPlayerFactory PlayerFactory { get; set; }

        public IMonsterFactory MonsterFactory { get; set; }
        
        public INpcFactory NpcFactory { get; set; }

        public IMap Map { get; set; }

        public ISpawnCollection Spawns { get; set; }

        public IPathfinding Pathfinding { get; set; }

        public void Start()
        {
            Statistics.Start();

            dispatcher.Start();

            scheduler.Start();

            QueueForExecution( () =>
            {
                Logger.WriteLine(ServerName + " " + ServerVersion + " - An open Tibia server developed by mtanksl");

                Logger.WriteLine("Source code: https://github.com/mtanksl/OpenTibia");

                Logger.WriteLine();
                            
                using (Logger.Measure("Loading dlls") )
                {
                    PluginLoader.Start();
                }

                using (Logger.Measure("Loading values config") )
                {
                    Values.Start();
                }

                using (Logger.Measure("Loading server config") )
                {
                    Config.Start();
                }

                using (Logger.Measure("Loading quests config") )
                {
                    Quests.Start();
                }

                using (Logger.Measure("Loading outfits config") )
                {
                    Outfits.Start();
                }

                using (Logger.Measure("Loading game object scripts config") )
                {
                    GameObjectScripts.Start();
                }

                using (Logger.Measure("Loading items") )
                {
                    ItemFactory.Start(OtbFile.Load(PathResolver.GetFullPath("data/items/items.otb") ), 
                                      DatFile.Load(PathResolver.GetFullPath("data/items/tibia.dat") ), 
                                      ItemsFile.Load(PathResolver.GetFullPath("data/items/items.xml") ) );
                }

                using (Logger.Measure("Loading monsters") )
                {
                    MonsterFactory.Start(MonsterFile.Load(PathResolver.GetFullPath("data/monsters") ) );
                }

                using (Logger.Measure("Loading npcs") )
                {
                    NpcFactory.Start(NpcFile.Load(PathResolver.GetFullPath("data/npcs") ) );
                }

                using (Logger.Measure("Loading plugins config and plugins") )
                {
                    Plugins.Start();
                }

                using (Logger.Measure("Loading scripts config and scripts") )
                {
                    Scripts.Start();
                }

                OtbmFile otbmFile;

                using (Logger.Measure("Loading map") )
                {
                    otbmFile = OtbmFile.Load(PathResolver.GetFullPath("data/world/" + Config.MapName + ".otbm") );

                    Map.Start(otbmFile, 
                              HouseFile.Load(PathResolver.GetFullPath("data/world/" + otbmFile.MapInfo.HouseFile) ) );
                }

                using (Logger.Measure("Loading spawns") )
                {
                    Spawns.Start(SpawnFile.Load(PathResolver.GetFullPath("data/world/" + otbmFile.MapInfo.SpawnFile) ) );
                }

                if (Spawns.UnknownMonsters.Count > 0)
                {
                    Logger.WriteLine("Unable to load monsters: " + string.Join(", ", Spawns.UnknownMonsters), LogLevel.Warning);
                }

                if (Spawns.UnknownNpcs.Count > 0)
                {
                    Logger.WriteLine("Unable to load npcs: " + string.Join(", ", Spawns.UnknownNpcs), LogLevel.Warning);
                }

                using (Logger.Measure("Testing database") )
                {
                    if ( !Context.Current.Database.CanConnect() )
                    {
                        Logger.WriteLine("Unable to connect to database.", LogLevel.Error);
                    }
                    else
                    {
                        if (Config.DatabaseType == "memory")
                        {
                            Context.Current.Database.CreateDatabase(Config.GamePort);
                        }                        
                    }
                }

                if (Config.LoginMaxconnections > 0 && Config.LoginPort > 0)
                {
                    using (Logger.Measure("Updating message of the day") )
                    {
                        DbMotd motd = Context.Current.Database.MotdRepository.GetLastMessageOfTheDay();

                        if (motd == null || motd.Message != Config.Motd)
                        {
                            Context.Current.Database.MotdRepository.AddMessageOfTheDay(new DbMotd() { Message = Config.Motd } );

                            Context.Current.Database.Commit();
                        }
                    }

                    using (Logger.Measure("Updating worlds") )
                    {
                        foreach (var dbWorld in Context.Current.Database.WorldRepository.GetWorlds() )
                        {
                            var world = Config.Worlds.Where(w => w.Name == dbWorld.Name).FirstOrDefault();

                            if (world != null)
                            {
                                dbWorld.Ip = world.Ip;

                                dbWorld.Port = world.Port;
                            }
                        }

                        Context.Current.Database.Commit();
                    }
                }

                using (Logger.Measure("Loading houses") )
                {
                    foreach (var dbHouse in Context.Current.Database.HouseRepository.GetHouses() )
                    {
                        House house = Map.GetHouse( (ushort)dbHouse.Id);

                        if (house != null)
                        {
                            if (dbHouse.Owner != null)
                            {
                                house.Owner = dbHouse.Owner.Name;
                            }

                            foreach (var dbHouseAccessList in dbHouse.HouseAccessLists)
                            {
                                if (dbHouseAccessList.ListId == 0xFE)
                                {
                                    house.GetSubOwnersList().SetText(dbHouseAccessList.Text);
                                }
                                else if (dbHouseAccessList.ListId == 0xFF)
                                {
                                    house.GetGuestsList().SetText(dbHouseAccessList.Text);
                                }
                                else
                                {
                                    house.GetDoorList( (byte)dbHouseAccessList.ListId).SetText(dbHouseAccessList.Text);
                                }
                            }

                            void AddItems(Container parent, long sequenceId)
                            {
                                foreach (var dbHouseItem in dbHouse.HouseItems.Where(hi => hi.ParentId == sequenceId) )
                                {
                                    Item item = ItemFactory.Create( (ushort)dbHouseItem.OpenTibiaId, (byte)dbHouseItem.Count);

                                    ItemFactory.Attach(item);

                                    if (item is Container container)
                                    {
                                        AddItems(container, dbHouseItem.SequenceId);
                                    }

                                    parent.AddContent(item);
                                }
                            }

                            foreach (var dbHouseItem in dbHouse.HouseItems.Where(hi => ( (hi.ParentId >> 36) & 0x01) == 0x01) )
                            {
                                int x = (int)( ( (dbHouseItem.ParentId >> 20) & 0xFFFF) );

                                int y = (int)( ( (dbHouseItem.ParentId >> 4) & 0xFFFF) );

                                int z = (int)( (dbHouseItem.ParentId & 0xF) );

                                Tile tile = Map.GetTile(new Position(x, y, z) );

                                if (tile != null)
                                {
                                    //TODO: Warn about moveable items inside the house during map load

                                    Item item = ItemFactory.Create( (ushort)dbHouseItem.OpenTibiaId, (byte)dbHouseItem.Count);

                                    ItemFactory.Attach(item);

                                    if (item is Container container)
                                    {
                                        AddItems(container, dbHouseItem.SequenceId);
                                    }

                                    tile.AddContent(item);
                                }
                            }
                        }
                    }
                }

                return Promise.Completed;

            } ).Wait();

            using (Logger.Measure("Clean up") )
            {
                GC.Collect();

                GC.WaitForPendingFinalizers();
            }

            if (Config.LoginMaxconnections > 0 && Config.LoginPort > 0)
            {
                loginServer = new Listener(socket => new LoginConnection(this, socket) );

                loginServer.Start(Config.LoginMaxconnections, Config.LoginPort);
            }

            if (Config.GameMaxConnections > 0 && Config.GamePort > 0)
            {
                gameServer = new Listener(socket => new GameConnection(this, socket) );

                gameServer.Start(Config.GameMaxConnections, Config.GamePort);
            }

            if (Config.InfoMaxConnections > 0 && Config.InfoPort > 0)
            {
                infoServer = new Listener(socket => new InfoConnection(this, socket) );

                infoServer.Start(Config.InfoMaxConnections, Config.InfoPort);
            }

            Status = ServerStatus.Running;

            Logger.WriteLine("Server status: running.");
        }

        private Dictionary<string, SchedulerEvent> schedulerEvents = new Dictionary<string, SchedulerEvent>();

        private Stopwatch ticks = new Stopwatch();

        public void Post(Context previousContext, Action run)
        {
            DispatcherEvent dispatcherEvent = new DispatcherEvent( () =>
            {
                ticks.Restart();

                try
                {
                    using (var context = new Context(this, previousContext) )
                    {
                        using (var scope = new Scope<Context>(context) )
                        {
                            run();

                            context.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.ToString(), LogLevel.Error);
                }

                ticks.Stop();

                Statistics.IncreaseProcessingTime(ticks.ElapsedTicks);
            } );

            dispatcher.QueueForExecution(dispatcherEvent);
        }

        public Promise QueueForExecution(Func<Promise> run)
        {
            return Promise.Run( (resolve, reject) =>
            {
                Context previousContext = Context.Current;

                DispatcherEvent dispatcherEvent = new DispatcherEvent( () =>
                {
                    ticks.Restart();

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
                                
                        reject(ex);
                    }

                    ticks.Stop();

                    Statistics.IncreaseProcessingTime(ticks.ElapsedTicks);
                } );

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
                    ticks.Restart();

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
                    
                        reject(ex);
                    }

                    ticks.Stop();

                    Statistics.IncreaseProcessingTime(ticks.ElapsedTicks);
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

        public void ReloadPlugins()
        {
            QueueForExecution( () =>
            {
                using (Logger.Measure("Reloading plugins config and plugins") )
                {
                    try
                    {
                        var plugin = new PluginCollection(this);

                        plugin.Start();

                        Plugins.Stop();

                        Plugins.Dispose();

                        Plugins = plugin;

                        Context.Current.AddEvent(GlobalServerReloadedEventArgs.Instance);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine(ex.ToString(), LogLevel.Error);
                    }
                }

                return Promise.Completed;

            } ).Wait();
        }

        public void KickAll()
        {
            QueueForExecution( () =>
            {
                List<Promise> promises = new List<Promise>();

                foreach (var observer in Context.Current.Server.GameObjects.GetPlayers() )
                {
                    promises.Add(Context.Current.AddCommand(new CreatureDestroyCommand(observer) ) );
                }

                return Promise.WhenAll(promises.ToArray() );

            } ).Wait();

            Logger.WriteLine("Kick all complete.");
        }

        public void Save()
        {
            QueueForExecution( () =>
            {
                using (Logger.Measure("Saving players") )
                {
                    Player[] players = GameObjectPool.GetPlayers().ToArray();

                    if (players.Length > 0)
                    {
                        DbPlayer[] dbPlayers = Context.Current.Database.PlayerRepository.GetPlayerByIds(players.Select(p => p.DatabasePlayerId).ToArray() );

                        foreach (var item in players.GroupJoin(dbPlayers, p => p.DatabasePlayerId, p => p.Id, (player, dbPlayers) => new { Player = player, DbPlayer = dbPlayers.FirstOrDefault() } ) )
                        {
                            Player player = item.Player;

                            DbPlayer dbPlayer = item.DbPlayer;

                            if (dbPlayer != null)
                            {
                                PlayerFactory.Save(dbPlayer, player);
                            }
                        }                   
                    }
                }

                using (Logger.Measure("Saving houses") )
                {
                    House[] houses = Map.GetHouses().ToArray();

                    if (houses.Length > 0)
                    {
                        DbHouse[] dbHouses = Context.Current.Database.HouseRepository.GetHouses();

                        foreach (var item in houses.GroupJoin(dbHouses, h => h.Id, h => h.Id, (house, dbHouses) => new { House = house, DbHouse = dbHouses.FirstOrDefault() } ) )
                        {
                            House house = item.House;

                            DbHouse dbHouse = item.DbHouse;

                            if (dbHouse == null)
                            {
                                dbHouse = new DbHouse()
                                {
                                    Id = house.Id
                                };

                                Context.Current.Database.HouseRepository.AddHouse(dbHouse);
                            }
                                                          
                            dbHouse.HouseAccessLists.Clear();
                            
                            if (house.Owner == null)
                            {
                                dbHouse.OwnerId = null;
                            }
                            else
                            {
                                DbPlayer dbPlayer = Context.Current.Database.PlayerRepository.GetPlayerByName(house.Owner); //TODO: Improve performance

                                if (dbPlayer != null)
                                {
                                    dbHouse.OwnerId = dbPlayer.Id;
                                }
                            }

                            HouseAccessList subOwnersList = house.GetSubOwnersList();

                            if (subOwnersList.Text != null)
                            {
                                dbHouse.HouseAccessLists.Add(new DbHouseAccessList()
                                {
                                    HouseId = house.Id,

                                    ListId = 0xFE,

                                    Text = subOwnersList.Text
                                } );
                            }

                            HouseAccessList guestsList = house.GetGuestsList();

                            if (guestsList.Text != null)
                            {
                                dbHouse.HouseAccessLists.Add(new DbHouseAccessList()
                                {
                                    HouseId = house.Id,

                                    ListId = 0xFF,

                                    Text = guestsList.Text
                                } );
                            }

                            foreach (var doorList in house.GetDoorsList() )
                            {
                                if (doorList.Value.Text != null)
                                {
                                    dbHouse.HouseAccessLists.Add(new DbHouseAccessList()
                                    {
                                        HouseId = house.Id,

                                        ListId = doorList.Key,

                                        Text = doorList.Value.Text
                                    } );
                                }
                            }

                            long sequenceId = 1;

                            void AddItems(long parentId, Item item)
                            {
                                DbHouseItem dbHouseItem = new DbHouseItem()
                                {
                                    HouseId = dbHouse.Id,

                                    SequenceId = sequenceId++,

                                    ParentId = parentId,

                                    OpenTibiaId = item.Metadata.OpenTibiaId,

                                    Count = item is StackableItem stackableItem ? stackableItem.Count :

                                            item is FluidItem fluidItem ? (int)fluidItem.FluidType :

                                            item is SplashItem splashItem ? (int)splashItem.FluidType : 1
                                };

                                dbHouse.HouseItems.Add(dbHouseItem);

                                if (item is Container container)
                                {
                                    foreach (var child in container.GetItems().Reverse() )
                                    {
                                        AddItems(dbHouseItem.SequenceId, child);
                                    }
                                }
                            }

                            dbHouse.HouseItems.Clear();

                            foreach (var tile in house.GetTiles() )
                            {
                                foreach (var moveable in tile.GetItems().Where(i => !i.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) ).Reverse() )
                                {
                                    AddItems( (long)0x01 << 36 | (long)tile.Position.X << 20 | (long)tile.Position.Y << 4 | (long)tile.Position.Z, moveable);
                                }
                            }
                        }
                    }
                }

                Context.Current.Database.Commit();

                return Promise.Completed;

            } ).Wait();

            Logger.WriteLine("Save complete.");
        }

        public void Pause()
        {
            if (Status == ServerStatus.Running)
            {
                Status = ServerStatus.Paused;

                Logger.WriteLine("Server status: paused.");
            }
        }

        public void Continue()
        {
            if (Status == ServerStatus.Paused)
            {
                Status = ServerStatus.Running;

                Logger.WriteLine("Server status: running.");
            }
        }

        public void Stop()
        {
            QueueForExecution( () =>
            {
                Plugins.Stop();

                Scripts.Stop();

                Spawns.Stop();

                return Promise.Completed;

            } ).Wait();

            if (loginServer != null)
            {
                loginServer.Stop();
            }

            if (gameServer != null)
            {
                gameServer.Stop();
            }

            if (infoServer != null)
            {
                infoServer.Stop();
            }

            scheduler.Stop();

            dispatcher.Stop();

            Status = ServerStatus.Stopped;

            Logger.WriteLine("Server status: stopped.");
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
                    if (Values != null)
                    {
                        Values.Dispose();
                    }

                    if (Config != null)
                    {
                        Config.Dispose();
                    }

                    if (Quests != null)
                    {
                        Quests.Dispose();
                    }

                    if (Outfits != null)
                    {
                        Outfits.Dispose();
                    }

                    if (GameObjectScripts != null)
                    {
                        GameObjectScripts.Dispose();
                    }

                    if (Plugins != null)
                    {
                        Plugins.Dispose();
                    }

                    if (Scripts != null)
                    {
                        Scripts.Dispose();
                    }

                    if (LuaScripts != null)
                    {
                        LuaScripts.Dispose();
                    }

                    if (loginServer != null)
                    {
                        loginServer.Dispose();
                    }

                    if (gameServer != null)
                    {
                        gameServer.Dispose();
                    }

                    if (infoServer != null)
                    {
                        infoServer.Dispose();
                    }

                    if (PluginLoader != null)
                    {
                        PluginLoader.Dispose();
                    }
                }
            }
        }
    }
}