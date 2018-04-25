namespace OpenTibia
{
    public class SelectOutfit
    {
        public SelectOutfit(ushort outfitId, string name, Addons addons)
        {
            this.OutfitId = outfitId;

            this.Name = name;

            this.Addons = addons;
        }

        public ushort OutfitId { get; set; }

        public string Name { get; set; }

        public Addons Addons { get; set; }
    }
}