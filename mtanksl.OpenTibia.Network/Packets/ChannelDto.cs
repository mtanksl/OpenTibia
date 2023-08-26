namespace OpenTibia.Network.Packets
{
    public class ChannelDto
    {
        public ChannelDto(ushort id, string name)
        {
            this.Id = id;

            this.Name = name;
        }

        public ushort Id { get; set; }

        public string Name { get; set; }
    }
}