namespace OpenTibia.Common.Objects
{
    public class Monster : Creature
    {
        public Monster(MonsterMetadata metadata)
        {
            Name = metadata.Name;

            Health = metadata.Health;

            MaxHealth = metadata.MaxHealth;

            Outfit = metadata.Outfit;

            Speed = metadata.Speed;

            this.metadata = metadata;
        }

        private MonsterMetadata metadata;

        public MonsterMetadata Metadata
        {
            get
            {
                return metadata;
            }
        }
    }
}