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
using OpenTibia.Network.Sockets;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public enum ServerStatus
    {
        /// <summary>
        /// It is not running.
        /// </summary>
        Stopped,

        /// <summary>
        /// It is running.
        /// </summary>
        Running,

        /// <summary>
        /// It is running, but not accepting new players.
        /// </summary>
        Paused
    }

    public class Server : IServer
    {
        public Server()
        {
            dispatcher = new Dispatcher();

            scheduler = new Scheduler(dispatcher);

            loginServer = new Listener(socket => new LoginConnection(this, socket) );

            gameServer = new Listener(socket => new GameConnection(this, socket) );

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

            GameObjectPool = new GameObjectPool();

            GameObjects = new GameObjectCollection();

            GameObjectComponents = new GameObjectComponentCollection();

            GameObjectEventHandlers = new GameObjectEventHandlerCollection();

            CommandHandlers = new CommandHandlerCollection();

            EventHandlers = new EventHandlerCollection();

            LuaScripts = new LuaScriptCollection(this);

            Config = new Config(this);

            Quests = new QuestCollection(this);

            Outfits = new OutfitCollection(this);

            Plugins = new PluginCollection(this);

            ItemFactory = new ItemFactory(this);

            PlayerFactory = new PlayerFactory(this);

            MonsterFactory = new MonsterFactory(this);

            NpcFactory = new NpcFactory(this);

            Map = new Map(this);

            Pathfinding = new Pathfinding(Map);

            Scripts = new ScriptCollection();
        }

        ~Server()
        {
            Dispose(false);
        }

        public ServerStatus Status { get; private set; }

        private Dispatcher dispatcher;

        private Scheduler scheduler;

        private Listener loginServer;

        private Listener gameServer;

        public DatabaseFactory DatabaseFactory { get; set; }

        public Logger Logger { get; set; }

        public PathResolver PathResolver { get; set; }

        public Randomization Randomization { get; set; }

        public Clock Clock { get; set; }

        public RateLimiting RateLimiting { get; set; }

        public WaitingList WaitingList { get; set; }

        public ChannelCollection Channels { get; set; }

        public RuleViolationCollection RuleViolations { get; set; }

        public GuildCollection Guilds { get; set; }

        public PartyCollection Parties { get; set; }

        public TradingCollection Tradings { get; set; }

        public NpcTradingCollection NpcTradings { get; set; }

        public GameObjectPool GameObjectPool { get; set; }

        public GameObjectCollection GameObjects { get; set; }

        public GameObjectComponentCollection GameObjectComponents { get; set; }

        public GameObjectEventHandlerCollection GameObjectEventHandlers { get; set; }

        public CommandHandlerCollection CommandHandlers { get; set; }

        public EventHandlerCollection EventHandlers { get; set; }

        public LuaScriptCollection LuaScripts { get; set; }

        public Config Config { get; set; }

        public QuestCollection Quests { get; set; }

        public OutfitCollection Outfits { get; set; }

        public PluginCollection Plugins { get; set; }

        public ItemFactory ItemFactory { get; set; }

        public PlayerFactory PlayerFactory { get; set; }

        public MonsterFactory MonsterFactory { get; set; }
        
        public NpcFactory NpcFactory { get; set; }

        public Map Map { get; set; }

        public Pathfinding Pathfinding { get; set; }

        public ScriptCollection Scripts { get; set; }

        public void Start()
        {
            dispatcher.Start();

            scheduler.Start();

            QueueForExecution( () =>
            {
                Logger.WriteLine("An open Tibia server developed by mtanksl");

                Logger.WriteLine("Source code: https://github.com/mtanksl/OpenTibia");

                Logger.WriteLine();
                               
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

                using (Logger.Measure("Loading plugins config") )
                {
                    Plugins.Start();
                }

                using (Logger.Measure("Loading items") )
                {
                    ItemFactory.Start(OtbFile.Load(PathResolver.GetFullPath("data/items/items.otb") ), 
                                      DatFile.Load(PathResolver.GetFullPath("data/items/tibia.dat") ), 
                                      ItemsFile.Load(PathResolver.GetFullPath("data/items/items.xml") ) );
                }

                using (Logger.Measure("Loading players") )
                {
                    PlayerFactory.Start();
                }

                using (Logger.Measure("Loading monsters") )
                {
                    MonsterFactory.Start(MonsterFile.Load(PathResolver.GetFullPath("data/monsters") ) );
                }

                using (Logger.Measure("Loading npcs") )
                {
                    NpcFactory.Start(NpcFile.Load(PathResolver.GetFullPath("data/npcs") ) );
                }

                using (Logger.Measure("Loading map") )
                {
                    Map.Start(OtbmFile.Load(PathResolver.GetFullPath("data/world/map.otbm") ), 
                              SpawnFile.Load(PathResolver.GetFullPath("data/world/map-spawn.xml") ), 
                              HouseFile.Load(PathResolver.GetFullPath("data/world/map-house.xml") ) );
                }

                if (Map.UnknownMonsters.Count > 0)
                {
                    Logger.WriteLine("Unable to load monsters: " + string.Join(", ", Map.UnknownMonsters), LogLevel.Warning);
                }

                if (Map.UnknownNpcs.Count > 0)
                {
                    Logger.WriteLine("Unable to load npcs: " + string.Join(", ", Map.UnknownNpcs), LogLevel.Warning);
                }

                using (Logger.Measure("Loading scripts") )
                {
                    Scripts.Start();
                }

                using (Logger.Measure("Testing database") )
                {
                    if ( !Context.Current.Database.DatabaseContext.Database.CanConnect() )
                    {
                        Logger.WriteLine("Unable to connect to database.", LogLevel.Error);
                    }
                    else
                    {
                        if (Config.DatabaseType == "memory")
                        {
                            Context.Current.Database.DatabaseContext.Database.EnsureDeleted();

                            Context.Current.Database.DatabaseContext.Motd.Add(new DbMotd() { Id = 0, Message = "An open Tibia server developed by mtanksl" } );

                            Context.Current.Database.DatabaseContext.Accounts.Add(new DbAccount() { Id = 1, Name = "1", Password = "1", PremiumDays = 0 } );

                            Context.Current.Database.DatabaseContext.Worlds.Add(new DbWorld() { Id = 1, Name = "Cormaya", Ip = "127.0.0.1", Port = Config.GamePort } );

                            Context.Current.Database.DatabaseContext.Players.Add(new DbPlayer() { Id = 1, AccountId = 1, WorldId = 1, Name = "Gamemaster", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 266, OutfitId = 266, BaseSpeed = 2218, Speed = 2218, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, Stamina = 2520, Rank = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

                            Context.Current.Database.DatabaseContext.Players.Add(new DbPlayer() { Id = 2, AccountId = 1, WorldId = 1, Name = "Knight", Health = 1565, MaxHealth = 1565, Direction = 2, BaseOutfitId = 131, OutfitId = 131, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 550, MaxMana = 550, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 1, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

                            Context.Current.Database.DatabaseContext.Players.Add(new DbPlayer() { Id = 3, AccountId = 1, WorldId = 1, Name = "Paladin", Health = 1105, MaxHealth = 1105, Direction = 2, BaseOutfitId = 129, OutfitId = 129, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 1470, MaxMana = 1470, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 2, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

                            Context.Current.Database.DatabaseContext.Players.Add(new DbPlayer() { Id = 4, AccountId = 1, WorldId = 1, Name = "Sorcerer", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, OutfitId = 130, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 4, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 });

                            Context.Current.Database.DatabaseContext.Players.Add(new DbPlayer() { Id = 5, AccountId = 1, WorldId = 1, Name = "Druid", Health = 645, MaxHealth = 645, Direction = 2, BaseOutfitId = 130, OutfitId = 130, BaseSpeed = 418, Speed = 418, Experience = 15694800, Level = 100, Mana = 2850, MaxMana = 2850, Soul = 100, Capacity = 139000, Stamina = 2520, Vocation = 3, SpawnX = 921, SpawnY = 771, SpawnZ = 6, TownX = 921, TownY = 771, TownZ = 6 } );

                            Context.Current.Database.Commit();
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

            loginServer.Start(Config.LoginMaxconnections, Config.LoginPort);

            gameServer.Start(Config.GameMaxConnections, Config.GamePort);

            Status = ServerStatus.Running;

            Logger.WriteLine("Server status: running.");
        }

        private Dictionary<string, SchedulerEvent> schedulerEvents = new Dictionary<string, SchedulerEvent>();

        public void Post(Context previousContext, Action run)
        {
            DispatcherEvent dispatcherEvent = new DispatcherEvent( () =>
            {
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
                Player[] players = GameObjectPool.GetPlayers().ToArray();

                if (players.Length > 0)
                {
                    DbPlayer[] dbPlayers = Context.Current.Database.PlayerRepository.GetPlayerByIds(players.Select(p => p.DatabasePlayerId).ToArray() );

                    foreach (var item in players.GroupJoin(dbPlayers, p => p.DatabasePlayerId, p => p.Id, (player, dbPlayers) => new { Player = player, DbPlayer = dbPlayers.First() } ) )
                    {
                        PlayerFactory.Save(item.DbPlayer, item.Player);
                    }

                    Context.Current.Database.Commit();
                }

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
                           
                return Promise.Completed;

            } ).Wait();

            loginServer.Stop();

            gameServer.Stop();

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
                    if (Plugins != null)
                    {
                        Plugins.Dispose();
                    }

                    if (Quests != null)
                    {
                        Quests.Dispose();
                    }

                    if (Config != null)
                    {
                        Config.Dispose();
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
                }
            }
        }
    }
}