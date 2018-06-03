namespace OpenTibia.Network.Packets
{
    public class Channel
    {
        public Channel(ushort id, string name)
        {
            this.Id = id;

            this.Name = name;
        }

        public ushort Id { get; set; }

        public string Name { get; set; }
    }
}