using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class PartyCollection
    {
        private List<Party> parties = new List<Party>();

        public int Count
        {
            get
            {
                return parties.Count;
            }
        }

        public void AddParty(Party party)
        {
            parties.Add(party);
        }

        public void RemoveParty(Party party)
        {
            parties.Remove(party);
        }

        public Party GetPartyByLeader(Player leader)
        {
            return GetParties()
                .Where(c => c.Leader == leader)
                .FirstOrDefault();
        }

        public Party GetPartyThatContainsMember(Player player)
        {
            return GetParties()
                .Where(c => c.ContainsMember(player) )
                .FirstOrDefault();
        }

        public IEnumerable<Party> GetPartyThatContainsInvitation(Player player)
        {
            return GetParties()
                .Where(c => c.ContainsInvitation(player) )
                .ToList();
        }

        public IEnumerable<Party> GetParties()
        {
            return parties;
        }
    }
}