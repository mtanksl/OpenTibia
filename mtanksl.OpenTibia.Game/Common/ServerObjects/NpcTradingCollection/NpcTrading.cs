using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class NpcTrading
    {
        public Npc OfferNpc { get; set; }

        public List<OfferDto> Offers { get; set; }

        public Player CounterOfferPlayer { get; set; }
    }
}