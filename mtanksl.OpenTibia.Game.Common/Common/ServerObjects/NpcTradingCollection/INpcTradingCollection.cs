using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface INpcTradingCollection
    {
        int Count { get; }

        void AddTrading(NpcTrading trading);

        void RemoveTrading(NpcTrading trading);

        IEnumerable<NpcTrading> GetTradingByOfferNpc(Npc offerNpc);

        NpcTrading GetTradingByCounterOfferPlayer(Player counterOfferPlayer);

        IEnumerable<NpcTrading> GetTradings();
    }
}