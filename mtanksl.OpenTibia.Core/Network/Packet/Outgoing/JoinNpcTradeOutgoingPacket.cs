using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia
{
    public class JoinNpcTradeOutgoingPacket : IOutgoingPacket
    {
        public JoinNpcTradeOutgoingPacket(uint money, List<CounterOffer> offers)
        {
            this.Money = money;

            this.Offers = offers;
        }

        public uint Money { get; set; }

        private List<CounterOffer> offers;

        public List<CounterOffer> Offers
        {
            get
            {
                return offers ?? ( offers = new List<CounterOffer>() );
            }
            set
            {
                offers = value;
            }
        }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7B );

            writer.Write(Money);

            writer.Write( (byte)Offers.Count );

            foreach (var offer in Offers)
            {
                writer.Write(offer.ItemId);

                writer.Write(offer.Count);
            }
        }
    }
}