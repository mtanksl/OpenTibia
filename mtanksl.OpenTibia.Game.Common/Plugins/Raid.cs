namespace OpenTibia.Game.Plugins
{
    public class Raid
    {
        public string Name { get; set; }

        public bool Repeatable { get; set; }

        public int Cooldown { get; set; }

        public double Chance { get; set; }
    }
}