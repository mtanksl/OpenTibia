using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class JoinNpcTradeOutgoingPacket : IOutgoingPacket
    {
        public JoinNpcTradeOutgoingPacket(uint money, List<CounterOfferDto> offers)
        {
            this.Money = money;

            this.Offers = offers;
        }

        public uint Money { get; set; }

        private List<CounterOfferDto> offers;

        public List<CounterOfferDto> Offers
        {
            get
            {
                return offers ?? ( offers = new List<CounterOfferDto>() );
            }
            set
            {
                offers = value;
            }
        }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7B );

            writer.Write(Money);

            writer.Write( (byte)Offers.Count );

            foreach (var offer in Offers)
            {
                writer.Write(offer.TibiaId);

                writer.Write(offer.Count);
            }
        }
    }
}