namespace OpenTibia.Common.Objects
{
    public class Monster : Creature
    {
        public Monster(MonsterMetadata metadata)
        {
            Name = metadata.NameDisplayed ?? metadata.Name;

            Health = metadata.Health;

            MaxHealth = metadata.MaxHealth;

            BaseLight = metadata.Light;

            BaseOutfit = metadata.Outfit;

            BaseSpeed = metadata.Speed;

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