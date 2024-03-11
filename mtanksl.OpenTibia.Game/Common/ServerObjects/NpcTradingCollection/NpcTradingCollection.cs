using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class NpcTradingCollection
    {
        private List<NpcTrading> tradings = new List<NpcTrading>();

        public int Count
        {
            get 
            {
                return tradings.Count; 
            }
        }

        public void AddTrading(NpcTrading trading)
        {
            tradings.Add(trading);
        }

        public void RemoveTrading(NpcTrading trading)
        {
            tradings.Remove(trading);
        }
        
        public IEnumerable<NpcTrading> GetTradingByOfferNpc(Npc offerNpc)
        {          
            return GetTradings()
                .Where(t => t.OfferNpc == offerNpc);
        }

        public NpcTrading GetTradingByCounterOfferPlayer(Player counterOfferPlayer)
        {
            return GetTradings()
                .Where(t => t.CounterOfferPlayer == counterOfferPlayer)
                .FirstOrDefault();
        }

        public IEnumerable<NpcTrading> GetTradings()
        {
            return tradings;
        }
    }
}