namespace OpenTibia.Network.Packets
{
    public class CharacterDto
    {
        public CharacterDto(string name, string world, string ip, ushort port, bool previewState)
        {
            this.Name = name;

            this.World = world;

            this.Ip = ip;

            this.Port = port;

            this.PreviewState = previewState;
        }

        public string Name { get; set; }

        public string World { get; set; }

        public string Ip { get; set; }

        public ushort Port { get; set; }

        public bool PreviewState { get; set; }
    }
}