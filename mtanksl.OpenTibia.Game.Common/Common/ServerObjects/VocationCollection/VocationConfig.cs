namespace OpenTibia.Game.Common.ServerObjects
{
    public class VocationConfig
    {
        public byte Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CapacityPerLevel { get; set; }

        public int HealthPerLevel { get; set; }

        public int ManaPerLevel { get; set; }

        public int RegenerateHealth { get; set; }

        public int RegenerateHealthInSeconds { get; set; }

        public int RegenerateMana { get; set; }

        public int RegenerateManaInSeconds { get; set; }

        public int RegenerateSoul { get; set; }

        public int RegenerateSoulInSeconds { get; set; }

        public int SoulMax { get; set; }

        public VocationConstantsConfig VocationConstants { get; set; }
    }
}