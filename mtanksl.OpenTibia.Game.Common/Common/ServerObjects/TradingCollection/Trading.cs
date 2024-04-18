using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Trading
    {
        public Player OfferPlayer { get; set; }

        public Item Offer { get; set; }

        public List<Item> OfferIncludes { get; set; }

        public HashSet<Item> OfferIncludesLookup { get; set; }

        public bool OfferPlayerAccepted { get; set; }


        public Player CounterOfferPlayer { get; set; }

        public Item CounterOffer { get; set; }

        public List<Item> CounterOfferIncludes { get; set; }

        public HashSet<Item> CounterOfferIncludesLookup { get; set; }

        public bool CounterOfferPlayerAccepted { get; set; }
    }
}