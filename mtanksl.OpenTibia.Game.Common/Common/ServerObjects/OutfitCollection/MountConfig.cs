namespace OpenTibia.Game.Common.ServerObjects
{
    public class MountConfig
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public int Speed { get; set; }

        public bool Premium { get; set; }

        public bool AvailableAtOnce { get; set; }

        public int MinClientVersion { get; set; }
    }
}