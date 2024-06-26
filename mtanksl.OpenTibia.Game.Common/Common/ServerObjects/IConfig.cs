﻿using OpenTibia.Data.Models;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IConfig : IDisposable
    {
        int InfoMaxConnections { get; set; }
        int InfoPort { get; set; }

        string IPAddress { get; set; }
        int Port { get; set; }
        string ServerName { get; set; }
        string Location { get; set; }
        string Url { get; set; }
        string OwnerName { get; set; }
        string OwnerEmail { get; set; }
        string MapName { get; set; }
        string MapAuthor { get; set; }

        int LoginMaxconnections { get; set; }
        int LoginPort { get; set; }

        string Motd { get; set; }
        DbWorld[] Worlds { get; set; }

        int GameMaxConnections { get; set; }
        int GamePort { get; set; }

        int GameplayMaxPlayers { get; set; }
        bool GameplayPrivateNpcSystem { get; set; }
        bool GameplayLearnSpellFirst { get; set; }
        bool GameplayInfinitePotions { get; set; }
        bool GameplayInfiniteArrows { get; set; }
        bool GameplayInfiniteRunes { get; set; }
        int GameplayMaxVips { get; set; }
        int GameplayMaxDepotItems { get; set; }       
        int GameplayKickLostConnectionAfterMinutes { get; set; }
        int GameplayKickIdlePlayerAfterMinutes { get; set; }
        int GameplayMonsterDeSpawnRange { get; set; }
        int GameplayMonsterDeSpawnRadius { get; set; }
        int GameplayLootRate { get; set; }
        int GameplayExperienceRate { get; set; }

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