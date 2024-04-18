using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class GuildCollection : IGuildCollection
    {
        private List<Guild> guilds = new List<Guild>();

        public int Count
        {
            get
            {
                return guilds.Count;
            }
        }

        public void AddGuild(Guild guild)
        {
            guilds.Add(guild);
        }

        public void RemoveGuild(Guild guild)
        {
            guilds.Remove(guild);
        }
            
        public Guild GetGuildThatContainsMember(Player player)
        {
            return GetGuilds()
                .Where(c => c.ContainsMember(player) )
                .FirstOrDefault();
        }

        public IEnumerable<Guild> GetGuilds()
        {
            return guilds;
        }
    }
}