namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class LootItem
    {
        public ushort Id { get; set; }

        public int? CountMin { get; set; }
                  
        public int? CountMax { get; set; }

        public int KillsToGetOne { get; set; }
    }
}