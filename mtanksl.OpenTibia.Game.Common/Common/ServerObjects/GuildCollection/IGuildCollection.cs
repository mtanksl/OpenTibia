using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGuildCollection
    {
        int Count { get; }

        void AddGuild(Guild guild);

        void RemoveGuild(Guild guild);

        Guild GetGuildThatContainsMember(Player player);

        IEnumerable<Guild> GetGuilds();
    }
}