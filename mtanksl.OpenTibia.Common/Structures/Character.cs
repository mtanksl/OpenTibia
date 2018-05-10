namespace OpenTibia.Common.Structures
{
    public class Character
    {
        public Character(string name, string world, string ip, ushort port)
        {
            this.Name = name;

            this.World = world;

            this.Ip = ip;

            this.Port = port;
        }

        public string Name { get; set; }

        public string World { get; set; }

        public string Ip { get; set; }

        public ushort Port { get; set; }
    }
}