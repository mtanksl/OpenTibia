namespace OpenTibia.Network.Packets
{
    public class Quest
    {
        public Quest(ushort id, string name, bool completed)
        {
            this.Id = id;

            this.Name = name;

            this.Completed = completed;
        }

        public ushort Id { get; set; }

        public string Name { get; set; }

        public bool Completed { get; set; }
    }
}