using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class ChannelCollection
    {
        private List<Channel> channels = new List<Channel>
        {
            new Channel() { Id = 2, Name = "Tutor" },

            new Channel() { Id = 3, Name = "Rule Violations" },

            new Channel() { Id = 4, Name = "Gamemaster" },

            new Channel() { Id = 5, Name = "Game Chat" },

            new Channel() { Id = 6, Name = "Trade" },

            new Channel() { Id = 7, Name = "Trade-Rookgaard" },

            new Channel() { Id = 8, Name = "Real Life Chat" },

            new Channel() { Id = 9, Name = "Help" }
        };

        /// <exception cref="InvalidOperationException"></exception>
        
        private ushort GenerateId()
        {
            for (ushort id = 10; id < 65535; id++)
            {
                if ( !channels.Any(c => c.Id == id) )
                {
                    return id;
                }
            }

            throw new InvalidOperationException("Channel limit exceeded.");
        }

        public void AddChannel(Channel channel)
        {
            if (channel.Id == 0)
            {
                channel.Id = GenerateId();
            }

            channels.Add(channel);
        }

        public void RemoveChannel(Channel channel)
        {
            channels.Remove(channel);
        }

        public Channel GetChannel(int channelId)
        {
            return GetChannels()
                .Where(c => c.Id == channelId)
                .FirstOrDefault();
        }

        public PrivateChannel GetPrivateChannelByOwner(Player owner)
        {
            return GetPrivateChannels()
                .Where(c => c.Owner == owner)
                .FirstOrDefault();
        }

        public IEnumerable<Channel> GetChannels()
        {
            return channels;
        }

        public IEnumerable<PrivateChannel> GetPrivateChannels()
        {
            return GetChannels().OfType<PrivateChannel>();
        }

        public IEnumerable<GuildChannel> GetGuildChannels()
        {
            return GetChannels().OfType<GuildChannel>();
        }

        public IEnumerable<PartyChannel> GetPartyChannels()
        {
            return GetChannels().OfType<PartyChannel>();
        }
    }
}