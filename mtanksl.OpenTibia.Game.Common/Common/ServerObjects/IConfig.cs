using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IConfig : IDisposable
    {
        int LoginMaxconnections { get; set; }
        int LoginPort { get; set; }

        int GameMaxConnections { get; set; }
        int GamePort { get; set; }
        int GameMaxPlayers { get; set; }

        bool GameplayPrivateNpcSystem { get; set; }
        bool LearnSpellFirst { get; set; }
        bool GameplayInfinitePotions { get; set; }
        bool GameplayInfiniteArrows { get; set; }
        bool GameplayInfiniteRunes { get; set; }
        int GameplayMaxVips { get; set; }
        int GameplayMaxDepotItems { get; set; }
        int GameplayLootRate { get; set; }

        int SecurityMaxConnectionsWithSameIpAddress { get; set; }
        int SecurityConnectionsWithSameIpAddressAbuseBanMilliseconds { get; set; }

        int SecurityMaxConnections { get; set; }
        int SecurityMaxConnectionsPerMilliseconds { get; set; }
        int SecurityConnectionsAbuseBanMilliseconds { get; set; }

        int SecurityMaxPackets { get; set; }
        int SecurityMaxPacketsPerMilliseconds { get; set; }
        int SecurityPacketsAbuseBanMilliseconds { get; set; }

        int SecurityMaxLoginAttempts { get; set; }
        int SecurityMaxLoginAttemptsPerMilliseconds { get; set; }
        int SecurityLoginAttemptsAbuseBanMilliseconds { get; set; }

        int SocketReceiveTimeoutMilliseconds { get; set; }
        int SocketSendTimeoutMilliseconds { get; set; }
        int SecurityMaxSlowSockets { get; set; }
        int SecurityMaxSlowSocketsPerMilliseconds { get; set; }
        int SecuritySlowSocketsAbuseBanMilliseconds { get; set; }

        int SecurityMaxInvalidMessages { get; set; }
        int SecurityMaxInvalidMessagesPerMilliseconds { get; set; }
        int SecurityInvalidMessagesAbuseBanMilliseconds { get; set; }

        int SecurityMaxUnknownPackets { get; set; }
        int SecurityMaxUnknownPacketsPerMilliseconds { get; set; }
        int SecurityUnknownPacketsAbuseBanMilliseconds { get; set; }

        string DatabaseType { get; set; }
        string DatabaseSource { get; set; }
        string DatabaseHost { get; set; }
        int DatabasePort { get; set; }
        string DatabaseUser { get; set; }
        string DatabasePassword { get; set; }
        string DatabaseName { get; set; }

        void Start();

        object GetValue(string key);
    }
}