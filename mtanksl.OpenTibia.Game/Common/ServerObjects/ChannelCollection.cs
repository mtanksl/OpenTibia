using NLua;
using OpenTibia.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class ChannelCollection : IChannelCollection
    {
        private IServer server;

        public ChannelCollection(IServer server)
        {
            this.server = server;
        }

        ~ChannelCollection()
        {
            Dispose(false);
        }

        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/channels/config.lua"),
                server.PathResolver.GetFullPath("data/channels/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );

            foreach (LuaTable lChannel in ( (LuaTable)script["channels"] ).Values)
            {
                channels.Add(new Channel()
                {
                    Id = LuaScope.GetUInt16(lChannel["id"] ),

                    Name = LuaScope.GetString(lChannel["name"] ),

                    Flags = (ChannelFlags)LuaScope.GetUInt16(lChannel["flags"] )
                } );
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
      
        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(OutfitCollection) );
            }

            return script[key];
        }

        private uint statementId = 0;

        public uint GenerateStatementId(int databasePlayerId, string message)
        {
            statementId++;

            statements.Add(statementId, new Statement()
            {
                Id = statementId,

                DatabasePlayerId = databasePlayerId,

                Message = message,

                CreationDate = DateTime.UtcNow
            } );

            return statementId;
        }

        private Dictionary<uint, Statement> statements = new Dictionary<uint, Statement>();

        public Statement GetStatement(uint statementId)
        {
            Statement statement;

            statements.TryGetValue(statementId, out statement);

            return statement;
        }

        private List<Channel> channels = new List<Channel>();

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

        public PrivateChannel GetPrivateChannel(Player owner)
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
                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}