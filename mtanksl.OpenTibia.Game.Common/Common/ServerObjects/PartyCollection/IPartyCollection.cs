using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPartyCollection
    {
        int Count { get; }

        void AddParty(Party party);

        void RemoveParty(Party party);

        Party GetPartyByLeader(Player leader);

        Party GetPartyThatContainsMember(Player player);

        IEnumerable<Party> GetPartyThatContainsInvitation(Player player);

        IEnumerable<Party> GetParties();
    }
}