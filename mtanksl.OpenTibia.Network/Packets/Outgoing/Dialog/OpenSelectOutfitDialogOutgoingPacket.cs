using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenSelectOutfitDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenSelectOutfitDialogOutgoingPacket(Outfit outfit, List<SelectOutfit> outfits)
        {
            this.Outfit = outfit;

            this.Outfits = outfits;
        }

        public Outfit Outfit { get; set; }

        private List<SelectOutfit> outfits;

        public List<SelectOutfit> Outfits
        {
            get
            {
                return outfits ?? ( outfits = new List<SelectOutfit>() );
            }
            set
            {
                outfits = value;
            }
        }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xC8 );

            writer.Write(Outfit);
            
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