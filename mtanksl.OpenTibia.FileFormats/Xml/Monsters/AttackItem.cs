namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class AttackItem
    {
        public string Name { get; set; }

        public int Interval { get; set; }

        public double Chance { get; set; }

        public int? Min { get; set; }

        public int? Max { get; set; }
    }
}