using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Channel
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        private HashSet<Player> members = new HashSet<Player>();

        public int CountMembers
        {
            get
            {
                return members.Count;
            }
        }

        public void AddMember(Player member)
        {
            members.Add(member);
        }

        public void RemoveMember(Player member)
        {
            members.Remove(member);
        }

        public bool ContainerMember(Player member)
        {
            return members.Contains(member);
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

    public class PrivateChannel : Channel
    {
        public Player Owner { get; set; }

        private HashSet<Player> invitations = new HashSet<Player>();

        public int CountInvitations
        {
            get
            {
                return invitations.Count;
            }
        }

        public void AddInvitation(Player player)
        {
            invitations.Add(player);
        }

        public void RemoveInvitation(Player player)
        {
            invitations.Remove(player);
        }

        public bool ContainsInvitation(Player player)
        {
            return invitations.Contains(player);
        }

        public IEnumerable<Player> GetInvitations()
        {
            return invitations;
        }
    }
}