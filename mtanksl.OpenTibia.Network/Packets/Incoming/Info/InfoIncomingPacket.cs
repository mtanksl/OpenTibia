using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class InfoIncomingPacket : IIncomingPacket
    {
        public bool Xml { get; set; }

        public RequestedInfo RequestedInfo { get; set; }

        public string PlayerName { get; set; }

        public void Read(IByteArrayStreamReader reader)
        {
            Xml = reader.ReadByte() == 0xFF;

            if (Xml)
            {
                if (reader.ReadString(4) == "info")
                {
                    RequestedInfo = RequestedInfo.BasicInfo | RequestedInfo.OwnerInfo | RequestedInfo.MiscInfo | RequestedInfo.PlayersInfo | RequestedInfo.MapInfo | RequestedInfo.SoftwareInfo;
                }
            }
            else
            {
                RequestedInfo = (RequestedInfo)reader.ReadUShort();

                if (RequestedInfo.Is(RequestedInfo.PlayerStatusInfo) )
                {
                    PlayerName = reader.ReadString();
                }
            }
        }
    }
}