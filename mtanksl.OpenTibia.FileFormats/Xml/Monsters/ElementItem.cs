namespace OpenTibia.FileFormats.Xml.Monsters
{
    // This format does not make much sense. Only need 1 node with multiple attributes.

    public class ElementItem
    {
        public int? PhysicalPercent { get; set; }

        public int? Earthpercent { get; set; }

        public int? FirePercent { get; set; }

        public int? EnergyPercent { get; set; }

        public int? IcePercent { get; set; }

        public int? DeathPercent { get; set; }

        public int? HolyPercent { get; set; }

        public int? DrownPercent { get; set; }

        public int? ManaDrainPercent { get; set; }

        public int? LifeDrainPercent { get; set; }
    }
}