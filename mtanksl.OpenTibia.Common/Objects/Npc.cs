namespace OpenTibia.Common.Objects
{
    public class Npc : Creature
    {
        public Npc(NpcMetadata metadata)
        {
            Name = metadata.Name;

            Health = metadata.Health;

            MaxHealth = metadata.MaxHealth;

            Outfit = metadata.Outfit;

            Speed = metadata.Speed;

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