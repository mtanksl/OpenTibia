using OpenTibia.IO;

namespace OpenTibia.Otbm
{
    public class OtbmInfo
    {
        public static OtbmInfo Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            OtbmInfo otbmInfo = new OtbmInfo();

            stream.Seek(Origin.Current, 6);

            otbmInfo.OtbmVersion = (OtbmVersion)reader.ReadUInt();

            otbmInfo.Width = reader.ReadUShort();

            otbmInfo.Height = reader.ReadUShort();

            otbmInfo.MajorVersion = (OtbVersion)reader.ReadUInt();

            otbmInfo.MinorVersion = (TibiaVersion)reader.ReadUInt();

            return otbmInfo;
        }

        public OtbmVersion OtbmVersion { get; set; }

        public ushort Width { get; set; }

        public ushort Height { get; set; }

        public OtbVersion MajorVersion { get; set; }

        public TibiaVersion MinorVersion { get; set; }
    }
}