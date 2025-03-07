using OpenTibia.Common.Structures;
using OpenTibia.Game.Common.ServerObjects;
using System;

namespace OpenTibia.Game.Common
{
    public interface IServer : IDisposable
    {
        string ServerName { get; }

        Version ServerVersion { get; }

        ServerStatus Status { get; }

        IMessageCollectionFactory MessageCollectionFactory { get; set; }

        IClientFactory ClientFactory { get; set; }

        IDatabaseFactory DatabaseFactory { get; set; }

        IServerStatistics Statistics { get; set; }

        ILogger Logger { get; set; }

        IPathResolver PathResolver { get; set; }

        IRandomization Randomization { get; set; }

        Clock Clock { get; set; }

        IRateLimiting RateLimiting { get; set; }

        IWaitingList WaitingList { get; set; }

        IChannelCollection Channels { get; set; }

        IRuleViolationCollection RuleViolations { get; set; }

        IGuildCollection Guilds { get; set; }

        IPartyCollection Parties { get; set; }

        ITradingCollection Tradings { get; set; }

        INpcTradingCollection NpcTradings { get; set; }

        ICombatCollection Combats { get; set; }

        IGameObjectPool GameObjectPool { get; set; }

        IGameObjectCollection GameObjects { get; set; }

        IGameObjectComponentCollection GameObjectComponents { get; set; }

        IGameObjectEventHandlerCollection GameObjectEventHandlers { get; set; }

        ICommandHandlerCollection CommandHandlers { get; set; }

        IEventHandlerCollection EventHandlers { get; set; }

        IPositionalEventHandlerCollection PositionalEventHandlers { get; set; }

        ILuaScriptCollection LuaScripts { get; set; }

        IPluginLoader PluginLoader { get; set; }

        IValues Values { get; set; }

        IConfig Config { get; set; }

        IFeatures Features { get; set; }

        IQuestCollection Quests { get; set; }

        IOutfitCollection Outfits { get; set; }

        IVocationCollection Vocations { get; set; }

        IPluginCollection Plugins { get; set; }

        IScriptCollection Scripts { get; set; }

        IGameObjectScriptCollection GameObjectScripts { get; set; }

        IItemFactory ItemFactory { get; set; }

        IPlayerFactory PlayerFactory { get; set; }

        IMonsterFactory MonsterFactory { get; set; }

        INpcFactory NpcFactory { get; set; }

        IMap Map { get; set; }

        ISpawnCollection Spawns { get; set; }

        IRaidCollection Raids { get; set; }

        IPathfinding Pathfinding { get; set; }

        void Start();

        void Post(Context previousContext, Action run);

        Promise QueueForExecution(Func<Promise> run);

        Promise QueueForExecution(string key, TimeSpan executeIn, Func<Promise> run);

        bool CancelQueueForExecution(string key);

        void ReloadPlugins();

        void KickAll();

        void Save();

        void Clean();

        void Pause();

        void Continue();

        void Stop();
    }
}