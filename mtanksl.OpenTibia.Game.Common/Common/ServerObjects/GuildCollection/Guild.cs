using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Guild
    {
        public string Name { get; set; }

        private HashSet<Player> members = new HashSet<Player>();

        public int CountMembers
        {
            get
            {
                return members.Count;
            }
        }

        public void AddMember(Player player)
        {
            members.Add(player);
        }

        public void RemoveMember(Player player)
        {
            members.Remove(player);
        }

        public bool ContainsMember(Player player)
        {
            return members.Contains(player);
        }

        public IEnumerable<Player> GetMembers()
        {
            return members;
        }

        public string Leader { get; set; }

        public bool IsLeader(string playerName)
        {
            return Leader == playerName;
        }

        public HashSet<string> ViceLeaders { get; set; }

        public bool IsViceLeader(string playerName)
        {
            return ViceLeaders.Contains(playerName);
        }
    }
}