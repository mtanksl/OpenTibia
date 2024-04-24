namespace OpenTibia.Network.Packets
{
    public class ExtPlayersInfoDto
    {
        public ExtPlayersInfoDto(string name, uint level)
        {
            this.Name = name;

            this.Level = level;
        }

        public string Name { get; set; }

        public uint Level { get; set; }
    }
}