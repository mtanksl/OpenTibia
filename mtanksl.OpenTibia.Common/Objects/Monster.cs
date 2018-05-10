namespace OpenTibia.Common.Objects
{
    public class Monster : Creature
    {
        public Monster(MonsterMetadata metadata)
        {
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