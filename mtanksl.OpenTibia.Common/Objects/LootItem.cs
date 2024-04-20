namespace OpenTibia.Common.Objects
{
    public class LootItem
    {
        public ushort OpenTibiaId { get; set; }

        public int KillsToGetOne { get; set; }

        public int CountMin { get; set; }

        public int CountMax { get; set; }
    }
}