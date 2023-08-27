using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class TradingCollection
    {
        private List<Trading> tradings = new List<Trading>();

        public void AddTrading(Trading trading)
        {
            tradings.Add(trading);
        }

        public void RemoveTrading(Trading trading)
        {
            tradings.Remove(trading);
        }
        
        public Trading GetTradingByOfferPlayer(Player offerPlayer)
        {          
            return GetTradings()
                .Where(t => t.OfferPlayer == offerPlayer)
                .FirstOrDefault();
        }

        public Trading GetTradingByCounterOfferPlayer(Player counterOfferPlayer)
        {
            return GetTradings()
                .Where(t => t.CounterOfferPlayer == counterOfferPlayer)
                .FirstOrDefault();
        }

        public Trading GetTradingByOffer(Item offer)
        {
            return GetTradings()
                .Where(t => t.OfferIncludes.Contains(offer) )
                .FirstOrDefault();
        }

        public Trading GetTradingByCounterOffer(Item counterOffer)
        {
            return GetTradings()
                .Where(t => t.CounterOfferIncludes != null && 
                            t.CounterOfferIncludes.Contains(counterOffer) )
                .FirstOrDefault();
        }

        public IEnumerable<Trading> GetTradings()
        {
            return tradings;
        }
    }
}