using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game
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

        public override string ToString()
        {
            return "Name: " + Name;
        }
    }
}