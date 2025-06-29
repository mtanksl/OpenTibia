namespace OpenTibia.Network.Packets
{
    public class CharacterDto
    {
        public CharacterDto(string playerName, string worldName, string ip, ushort port, bool previewState)
        {
            this.PlayerName = playerName;

            this.WorldName = worldName;

            this.Ip = ip;

            this.Port = port;

            this.PreviewState = previewState;
        }

        public string PlayerName { get; set; }

        public string WorldName { get; set; }

        public string Ip { get; set; }

        public ushort Port { get; set; }

        public bool PreviewState { get; set; }
    }
}