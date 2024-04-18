using OpenTibia.Common.Structures;
using OpenTibia.Game.Common.ServerObjects;
using System;

namespace OpenTibia.Game.Common
{
    public interface IServer : IDisposable
    {
        ServerStatus Status { get; }

        IClientFactory ClientFactory { get; set; }

        IDatabaseFactory DatabaseFactory { get; set; }

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

        IGameObjectPool GameObjectPool { get; set; }

        IGameObjectCollection GameObjects { get; set; }

        IGameObjectComponentCollection GameObjectComponents { get; set; }

        IGameObjectEventHandlerCollection GameObjectEventHandlers { get; set; }

        ICommandHandlerCollection CommandHandlers { get; set; }

        IEventHandlerCollection EventHandlers { get; set; }

        ILuaScriptCollection LuaScripts { get; set; }

        IPluginLoader PluginLoader { get; set; }

        IConfig Config { get; set; }

        IQuestCollection Quests { get; set; }

        IOutfitCollection Outfits { get; set; }

        IPluginCollection Plugins { get; set; }

        IScriptCollection Scripts { get; set; }

        IItemFactory ItemFactory { get; set; }

        IPlayerFactory PlayerFactory { get; set; }

        IMonsterFactory MonsterFactory { get; set; }

        INpcFactory NpcFactory { get; set; }

        IMap Map { get; set; }

        IPathfinding Pathfinding { get; set; }

        void Start();

        void Post(Context previousContext, Action run);

        Promise QueueForExecution(Func<Promise> run);

        Promise QueueForExecution(string key, TimeSpan executeIn, Func<Promise> run);

        bool CancelQueueForExecution(string key);

        void KickAll();

        void Save();

        void Pause();

        void Continue();

        void Stop();
    }
}