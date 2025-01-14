using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IConfig : IDisposable
    {
        int InfoMaxConnections { get; set; }
        int InfoPort { get; set; }

        string IpAddress { get; set; }
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
        bool LoginAccountManagerEnabled { get; set; }
        bool LoginAccountManagerAllowChangePlayerName { get; set; }
        bool LoginAccountManagerAllowChangePlayerGender { get; set; }
        string LoginAccountManagerAccountName { get; set; }
        string LoginAccountManagerAccountPassword { get; set; }
        string LoginAccountManagerPlayerName { get; set; }
        Position LoginAccountManagerPlayerPosition { get; set; }
        Position LoginAccountManagerPlayerNewPosition { get; set; }
        string LoginAccountManagerWorldName { get; set; }
        string LoginAccountManagerIpAddress { get; set; }
        int LoginAccountManagerPort { get; set; }

        string Motd { get; set; }
        DbWorld[] Worlds { get; set; }

        int GameMaxConnections { get; set; }
        int GamePort { get; set; }

        int GameplayMaxPlayers { get; set; }
        bool GameplayPrivateNpcSystem { get; set; }
        bool GameplayLearnSpellFirst { get; set; }
        bool GameplayRemoveChargesFromPotions { get; set; }
        bool GameplayRemoveWeaponAmmunition { get; set; }
        bool GameplayRemoveChargesFromRunes { get; set; }
        bool GameplayRemoveWeaponCharges { get; set; }
        bool GameplayAllowChangeOutfit { get; set; }
        bool GameplayHotkeyAimbotEnabled { get; set; }
        bool GameplayShowOnlineStatusInCharlist { get; set; }
        bool GameplayAllowClones { get; set; }
        bool GameplayOnePlayerOnlinePerAccount { get; set; }
        bool GameplayReplaceKickOnLogin { get; set; }
        int GameplayVipFreeLimit { get; set; }
        int GameplayVipPremiumLimit { get; set; }
        int GameplayDepotFreeLimit { get; set; }       
        int GameplayDepotPremiumLimit { get; set; }       
        int GameplayKickLostConnectionAfterMinutes { get; set; }
        int GameplayKickIdlePlayerAfterMinutes { get; set; }
        int GameplayMonsterDeSpawnRange { get; set; }
        int GameplayMonsterDeSpawnRadius { get; set; }
        bool GameplayMonsterRemoveOnDeSpawn { get; set; }
        int GameplayLootRate { get; set; }
        int GameplayMoneyRate { get; set; }
        double GameplayExperienceRate { get; set; }
        ExperienceStagesConfig GameplayExperienceStages { get; set; }

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