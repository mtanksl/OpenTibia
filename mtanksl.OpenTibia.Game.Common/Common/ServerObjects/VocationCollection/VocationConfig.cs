namespace OpenTibia.Game.Common.ServerObjects
{
    public class VocationConfig
    {
        public byte Id { get; set; }

        public string Name { get; set; }

        public int CapacityPerLevel { get; set; }

        public int HealthPerLevel { get; set; }

        public int ManaPerLevel { get; set; }

        public int Health { get; set; }

        public int HealthDelayInSeconds { get; set; }

        public int Mana { get; set; }

        public int ManaDelayInSeconds { get; set; }
    }
}