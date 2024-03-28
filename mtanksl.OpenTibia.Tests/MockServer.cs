using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.MySql.Contexts;
using OpenTibia.Game;
using OpenTibia.Game.Commands;
using OpenTibia.Threading;
using System;

namespace mtanksl.OpenTibia.Tests
{
    public class MockServer : IServer
    {
        public MockServer()
        {
            DatabaseFactory = new DatabaseFactory(this, builder =>
            {
                return new InMemoryContext(builder.Options);
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

        public ServerStatus Status { get; private set; }

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
            Status = ServerStatus.Running;
        }

        public void Post(Context previousContext, Action run)
        {
            throw new NotImplementedException();
        }

        public Promise QueueForExecution(Func<Promise> run)
        {
            throw new NotImplementedException();
        }

        public Promise QueueForExecution(string key, TimeSpan executeIn, Func<Promise> run)
        {
            throw new NotImplementedException();
        }

        public bool CancelQueueForExecution(string key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void KickAll()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            Status = ServerStatus.Paused;
        }

        public void Continue()
        {
            Status = ServerStatus.Running;
        }
        public void Stop()
        {
            Status = ServerStatus.Stopped;
        }

        public void Dispose()
        {
            
        }
    }
}