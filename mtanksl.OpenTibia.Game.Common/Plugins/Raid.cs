namespace OpenTibia.Game.Plugins
{
    public class Raid
    {
        public string Name { get; set; }

        public bool Repeatable { get; set; }

        public int Interval { get; set; }

        public double Chance { get; set; }

        public bool Enabled { get; set; }
    }
}