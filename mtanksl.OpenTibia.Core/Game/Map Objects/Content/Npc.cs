namespace OpenTibia
{
    public class Npc : Creature
    {
        public Npc(NpcMetadata metadata) : base()
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