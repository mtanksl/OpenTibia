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
                .Where(r => r.OfferPlayer == offerPlayer)
                .FirstOrDefault();
        }

        public Trading GetTradingByCounterOfferPlayer(Player counterOfferPlayer)
        {
            return GetTradings()
                .Where(r => r.CounterOfferPlayer == counterOfferPlayer)
                .FirstOrDefault();
        }

        public IEnumerable<Trading> GetTradings()
        {
            return tradings;
        }
    }
}