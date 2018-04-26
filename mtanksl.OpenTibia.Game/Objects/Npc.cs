namespace OpenTibia.Game.Objects
{
    public class Npc : Creature
    {
        public Npc(NpcMetadata metadata)
        {
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