using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class InviteNpcTradeOutgoingPacket : IOutgoingPacket
    {
        public InviteNpcTradeOutgoingPacket(string npcName, List<OfferDto> offers)
        {
            this.NpcName = npcName;

            this.Offers = offers;
        }

        public string NpcName { get; set; }

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

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x7A );

            if (features.HasFeatureFlag(FeatureFlag.NameOnNpcTrade) )
            {
                writer.Write(NpcName);
            }

            if ( !features.HasFeatureFlag(FeatureFlag.InviteNpcTradeU16) )
            {
                writer.Write( (byte)Offers.Count);
            }
            else
            {
                writer.Write( (ushort)Offers.Count);
            }

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