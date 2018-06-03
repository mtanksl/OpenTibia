using OpenTibia.Common.Structures;

namespace OpenTibia.Network.Packets
{
    public class SelectOutfit
    {
        public SelectOutfit(ushort outfitId, string name, Addon addon)
        {
            this.OutfitId = outfitId;

            this.Name = name;

            this.Addon = addon;
        }

        public ushort OutfitId { get; set; }

        public string Name { get; set; }

        public Addon Addon { get; set; }
    }
}