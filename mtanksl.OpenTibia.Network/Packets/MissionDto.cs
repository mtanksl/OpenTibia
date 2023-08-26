namespace OpenTibia.Network.Packets
{
    public class MissionDto
    {
        public MissionDto(string name, string description)
        {
            this.Name = name;

            this.Description = description;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}