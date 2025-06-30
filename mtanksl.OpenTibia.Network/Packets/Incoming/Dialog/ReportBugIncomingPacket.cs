using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ReportBugIncomingPacket : IIncomingPacket
    {
        public string Message { get; set; }

        public BugReportType BugReportType { get; set; }

        public Position Position { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            if ( !features.HasFeatureFlag(FeatureFlag.ReportCoordinate) )
            {
                BugReportType = BugReportType.Other;
            }
            else
            {
                BugReportType = (BugReportType)reader.ReadByte();
            }

            Message = reader.ReadString();

            if (BugReportType == BugReportType.Map)
            {
                Position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );
            }
        }
    }
}