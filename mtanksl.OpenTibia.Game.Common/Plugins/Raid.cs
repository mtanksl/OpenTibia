namespace OpenTibia.Game.Plugins
{
    public class Raid
    {
        public string Name { get; set; }

        public bool Repeatable { get; set; }

        public int Cooldown { get; set; }

        public int Chance { get; set; }
    }
}