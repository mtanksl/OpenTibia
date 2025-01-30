using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class InviteNpcTradeOutgoingPacket : IOutgoingPacket
    {
        public InviteNpcTradeOutgoingPacket(List<OfferDto> offers)
        {
            this.Offers = offers;
        }

        private List<OfferDto> offers;

        public List<OfferDto> Offers
        {
            get
            {
                return offers ?? ( offers = new List<OfferDto>() );
            }
            set
            {
                offers = value;
            }
        }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x7A );

            writer.Write( (byte)Offers.Count );

            foreach (var offer in Offers)
            {
                writer.Write(offer.TibiaId);

                writer.Write(offer.Type); //TODO: NPC Trade Window % FluidColors.Length

                writer.Write(offer.Name);

                writer.Write(offer.Weight);

                writer.Write(offer.BuyPrice);

                writer.Write(offer.SellPrice);
            }
        }
    }
}