using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game
{
    public interface IServer : IDisposable
    {
        ServerStatus Status { get; }

        DispatcherContext DispatcherContext { get; set; }

        DatabaseFactory DatabaseFactory { get; set; }

        Logger Logger { get; set; }

        PathResolver PathResolver { get; set; }

        Randomization Randomization { get; set; }

        Clock Clock { get; set; }

        RateLimiting RateLimiting { get; set; }

        WaitingList WaitingList { get; set; }

        ChannelCollection Channels { get; set; }

        RuleViolationCollection RuleViolations { get; set; }

        GuildCollection Guilds { get; set; }

        PartyCollection Parties { get; set; }

        TradingCollection Tradings { get; set; }

        NpcTradingCollection NpcTradings { get; set; }

        GameObjectPool GameObjectPool { get; set; }

        GameObjectCollection GameObjects { get; set; }

        GameObjectComponentCollection GameObjectComponents { get; set; }

        GameObjectEventHandlerCollection GameObjectEventHandlers { get; set; }

        CommandHandlerCollection CommandHandlers { get; set; }

        EventHandlerCollection EventHandlers { get; set; }

        LuaScriptCollection LuaScripts { get; set; }

        Config Config { get; set; }

        QuestCollection Quests { get; set; }

        OutfitCollection Outfits { get; set; }

        PluginCollection Plugins { get; set; }

        ItemFactory ItemFactory { get; set; }

        PlayerFactory PlayerFactory { get; set; }

        MonsterFactory MonsterFactory { get; set; }

        NpcFactory NpcFactory { get; set; }

        Map Map { get; set; }

        Pathfinding Pathfinding { get; set; }

        ScriptCollection Scripts { get; set; }

        void Start();

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