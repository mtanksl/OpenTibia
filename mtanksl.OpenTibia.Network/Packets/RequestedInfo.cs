namespace OpenTibia.Network.Packets
{
    public enum RequestedInfo : ushort
    {
        None = 0,

        BasicInfo = 1,

        OwnerInfo = 2,

        MiscInfo = 4,

        PlayersInfo = 8,

        MapInfo = 16,

        ExtPlayersInfo = 32,

        PlayerStatusInfo = 64,

        SoftwareInfo = 128
    }

    public static class RequestedInfoExtensions
    {
        public static bool Is(this RequestedInfo flags, RequestedInfo flag)
        {
            return (flags & flag) == flag;
        }
    }
}