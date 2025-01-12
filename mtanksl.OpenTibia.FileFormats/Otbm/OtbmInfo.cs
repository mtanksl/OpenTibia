using OpenTibia.FileFormats.Otb;
using OpenTibia.IO;

namespace OpenTibia.FileFormats.Otbm
{
    public class OtbmInfo
    {
        public static OtbmInfo Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            OtbmInfo otbmInfo = new OtbmInfo();

            stream.Seek(Origin.Current, 1);

            otbmInfo.OtbmVersion = (OtbmVersion)reader.ReadUInt();

            otbmInfo.Width = reader.ReadUShort();

            otbmInfo.Height = reader.ReadUShort();

            otbmInfo.OtbVersion = (OtbVersion)reader.ReadUInt();

            otbmInfo.TibiaVersion = (TibiaVersion)reader.ReadUInt();

            return otbmInfo;
        }

        public static void Save(OtbmInfo otbmInfo, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)OtbmType.Root);

            writer.Write( (uint)otbmInfo.OtbmVersion);

            writer.Write( (ushort)otbmInfo.Width);

            writer.Write( (ushort)otbmInfo.Height);

            writer.Write( (uint)otbmInfo.OtbVersion);

            writer.Write( (uint)otbmInfo.TibiaVersion);
        }

        public OtbmVersion OtbmVersion { get; set; }

        public ushort Width { get; set; }

        public ushort Height { get; set; }

        public OtbVersion OtbVersion { get; set; }

        public TibiaVersion TibiaVersion { get; set; }
    }
}