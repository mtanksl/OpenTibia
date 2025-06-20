using OpenTibia.Common.Objects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IChannelCollection : IDisposable
    {
        void Start();

        object GetValue(string key);

        uint GenerateStatementId(int databasePlayerId, string message);

        Statement GetStatement(uint statementId);

        void AddChannel(Channel channel);

        void RemoveChannel(Channel channel);

        Channel GetChannel(int channelId);

        PrivateChannel GetPrivateChannel(Player owner);

        IEnumerable<Channel> GetChannels();

        IEnumerable<PrivateChannel> GetPrivateChannels();
    }
}