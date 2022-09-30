using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ReportRuleViolationIncomingPacket : IIncomingPacket
    {
        public string Name { get; set; }

        public string Comment { get; set; }

        public string Translation { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            switch (reader.ReadByte() )
            {
                case 0x00:

                    reader.ReadByte();

                    Name = reader.ReadString();

                    Comment = reader.ReadString();

                    Translation = reader.ReadString();

                    break;

                case 0x02:

                    Name = reader.ReadString();

                    Comment = reader.ReadString();

                    break;
            }
        }
    }
}