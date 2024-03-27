using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Party
    {
        public Player Leader { get; set; }

        public bool SharedExperienceEnabled { get; set; }

        private QueueHashSet<Player> members = new QueueHashSet<Player>();

        public int CountMembers
        {
            get
            {
                return members.Count;
            }
        }

        public Player NextMember()
        {
            return members.Peek();
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