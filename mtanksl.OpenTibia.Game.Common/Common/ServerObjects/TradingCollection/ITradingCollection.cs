using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ITradingCollection
    {
        int Count { get; }

        void AddTrading(Trading trading);

        void RemoveTrading(Trading trading);

        Trading GetTradingByOfferPlayer(Player offerPlayer);

        Trading GetTradingByCounterOfferPlayer(Player counterOfferPlayer);

        Trading GetTradingByOffer(Item offer);

        Trading GetTradingByCounterOffer(Item counterOffer);

        IEnumerable<Trading> GetTradings();
    }
}