namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IRateLimiting
    {
        bool IncreaseActiveConnection(string ipAddress);

        void DecreaseActiveConnection(string ipAddress);

        bool IsConnectionCountOk(string ipAddress);

        bool IsPacketCountOk(string ipAddress);

        bool IsLoginAttempsOk(string ipAddress);

        bool IncreaseSlowSocket(string ipAddress);

        bool IncreaseInvalidMessage(string ipAddress);

        bool IncreaseUnknownPacket(string ipAddress);
    }
}