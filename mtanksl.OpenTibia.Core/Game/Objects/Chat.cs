using System.Collections.Generic;

namespace OpenTibia
{
    public class Chat
    {
        public ushort Id { get; set; }

        public string Name { get; set; }


        protected HashSet<Player> players = new HashSet<Player>();

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        public bool ContainsPlayer(Player player)
        {
            return players.Contains(player);
        }

        public IEnumerable<Player> GetPlayers()
        {
            return players;
        }
    }

    public class GuildChat : Chat
    {
       
    }

    public class PartyChat : Chat
    {
        
    }

    public class PrivateChat : Chat
    {
        public Player Owner { get; set; }


        private HashSet<Player> invitations = new HashSet<Player>();

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