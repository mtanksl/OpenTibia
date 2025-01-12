using OpenTibia.IO;

namespace OpenTibia.FileFormats.Otb
{
    public class OtbInfo
    {
        public static OtbInfo Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            OtbInfo otbInfo = new OtbInfo();

            stream.Seek(Origin.Current, 1); // OtbType.Root

            otbInfo.Flags = reader.ReadUInt();

            stream.Seek(Origin.Current, 1); // OtbType.OtbInfo

            stream.Seek(Origin.Current, 2); // Length

            otbInfo.OtbVersion = (OtbVersion)reader.ReadUInt();

            otbInfo.TibiaVersion = (TibiaVersion)reader.ReadUInt();

            otbInfo.Revision = reader.ReadUInt();

            otbInfo.CsdVersion = reader.ReadCsd();

            return otbInfo;
        }

        public uint Flags { get; set; }

        public OtbVersion OtbVersion { get; set; }

        public TibiaVersion TibiaVersion { get; set; }

        public uint Revision { get; set; }

        public string CsdVersion { get; set; }
    }
}