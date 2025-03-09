using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenSelectOutfitDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenSelectOutfitDialogOutgoingPacket(Outfit outfit, List<OutfitDto> outfits)
        {
            this.Outfit = outfit;

            this.Outfits = outfits;
        }

        public Outfit Outfit { get; set; }

        private List<OutfitDto> outfits;

        public List<OutfitDto> Outfits
        {
            get
            {
                return outfits ?? ( outfits = new List<OutfitDto>() );
            }
            set
            {
                outfits = value;
            }
        }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xC8 );

            writer.Write(features, Outfit);

            if ( !features.HasFeatureFlag(FeatureFlag.NewOutfitProtocol) )
            {
                outfits.RemoveAll(o => o.OutfitId == Outfit.GamemasterBlue.Id);

                outfits.RemoveAll(o => o.OutfitId == Outfit.GamemasterRed.Id);

                outfits.RemoveAll(o => o.OutfitId == Outfit.GamemasterGreen.Id);

                if ( !features.HasFeatureFlag(FeatureFlag.LookTypeUInt16) )
                {
                    writer.Write( (byte)Outfits[0].OutfitId);

                    writer.Write( (byte)Outfits[Outfits.Count - 1].OutfitId);
                }
                else
                {
                    writer.Write(Outfits[0].OutfitId);

                    writer.Write(Outfits[Outfits.Count - 1].OutfitId);
                }
            }
            else
            {            
                writer.Write( (byte)Outfits.Count );

                foreach (var outfit in Outfits)
                {
                    writer.Write(outfit.OutfitId);

                    writer.Write(outfit.Name);

                    writer.Write( (byte)outfit.Addon );
                }
            }
        }
    }
}