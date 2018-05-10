using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class InviteNpcTrade : IOutgoingPacket
    {
        public InviteNpcTrade(List<Offer> offers)
        {
            this.Offers = offers;
        }

        private List<Offer> offers;

        public List<Offer> Offers
        {
            get
            {
                return offers ?? ( offers = new List<Offer>() );
            }
            set
            {
                offers = value;
            }
        }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7A );

            writer.Write( (byte)Offers.Count );

            foreach (var offer in Offers)
            {
                writer.Write(offer.ItemId);

                writer.Write(offer.Type);

                writer.Write(offer.Name);

                writer.Write(offer.Weight);

                writer.Write(offer.BuyPrice);

                writer.Write(offer.SellPrice);
            }
        }
    }
}