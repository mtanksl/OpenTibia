namespace OpenTibia.Network.Packets
{
    public class MountDto
    {
        public MountDto(ushort mountId, string name)
        {
            this.MountId = mountId;

            this.Name = name;
        }

        public ushort MountId { get; set; }

        public string Name { get; set; }
    }
}