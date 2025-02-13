namespace OpenTibia.Common.Objects
{
    public class Npc : Creature
    {
        public Npc(NpcMetadata metadata)
        {
            Name = metadata.NameDisplayed ?? metadata.Name;

            Health = metadata.Health;

            MaxHealth = metadata.MaxHealth;

            BaseLight = metadata.Light;

            BaseOutfit = metadata.Outfit;

            BaseSpeed = metadata.Speed;

            this.metadata = metadata;
        }

        private NpcMetadata metadata;

        public NpcMetadata Metadata
        {
            get
            {
                return metadata;
            }
        }
    }
}