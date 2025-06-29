using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class EnterGameIncomingPacket : IIncomingPacket
    {
        public OperatingSystem OperatingSystem { get; set; }

        public ushort ProtocolVersion { get; set; }

        public uint ClientVersion { get; set; }

        public ushort ContentRevision { get; set; }

        public uint TibiaDat { get; set; }

        public uint TibiaSpr { get; set; }

        public uint TibiaPic { get; set; }

        public uint[] Keys { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public byte LocaleId { get; set; }

        public string Locale { get; set; }

        public ushort TotalRam { get; set; }

        public string Cpu { get; set; }

        public ushort CpuClock { get; set; }

        public ushort CpuClock2 { get; set; }

        public string Gpu { get; set; }

        public ushort VideoRam { get; set; }

        public ushort ResolutionHorizontal { get; set; }

        public ushort ResolutionVertical { get; set; }

        public byte RefreshRate { get; set; }

        public string GraphicsDriver { get; set; }

        public string GraphicsVersion { get; set; }

        public string AuthenticatorCode { get; set; }

        public bool StayLoggedIn { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            OperatingSystem = (OperatingSystem)reader.ReadUShort();

            ProtocolVersion = reader.ReadUShort();

            if ( !features.HasFeatureFlag(FeatureFlag.ClientVersion) )
            {
                ClientVersion = ProtocolVersion;
            }
            else
            {
                ClientVersion = reader.ReadUInt();
            }

            if (features.HasFeatureFlag(FeatureFlag.ContentRevision) )
            {
                ContentRevision = reader.ReadUShort();

                reader.BaseStream.Seek(Origin.Current, 2); // ?
            }
            else
            {
                TibiaDat = reader.ReadUInt();
            }

            TibiaSpr = reader.ReadUInt();

            TibiaPic = reader.ReadUInt();

            if (features.HasFeatureFlag(FeatureFlag.PreviewState) )
            {
                reader.BaseStream.Seek(Origin.Current, 1); // ?
            }

            int position = reader.BaseStream.Position;

            reader.BaseStream.Seek(Origin.Current, 1); // 0x00

            Keys = new uint[]
            {
                reader.ReadUInt(), 
                
                reader.ReadUInt(),
                
                reader.ReadUInt(), 
                
                reader.ReadUInt()
            };

            if ( !features.HasFeatureFlag(FeatureFlag.AccountString) )
            {
                var account = reader.ReadUInt();

                if (account == 0)
                {
                    Account = "";
                }
                else
                {
                    Account = account.ToString();
                }
            }
            else
            {
                Account = reader.ReadString();
            }

            Password = reader.ReadString();

            if (features.HasFeatureFlag(FeatureFlag.AccountString) )
            {
                LocaleId = reader.ReadByte();

                Locale = reader.ReadString(3);

                TotalRam = reader.ReadUShort();

                reader.BaseStream.Seek(Origin.Current, 6); // ?

                Cpu = reader.ReadString(9);

                reader.BaseStream.Seek(Origin.Current, 2); // ?

                CpuClock = reader.ReadUShort();

                CpuClock2 = reader.ReadUShort();

                reader.BaseStream.Seek(Origin.Current, 4); // ?

                Gpu = reader.ReadString(9);

                VideoRam = reader.ReadUShort();

                ResolutionHorizontal = reader.ReadUShort();

                ResolutionVertical = reader.ReadUShort();

                RefreshRate = reader.ReadByte();
            }

            reader.BaseStream.Seek(Origin.Current, 128 - (reader.BaseStream.Position - position) ); // RSA Padding

            if (features.HasFeatureFlag(FeatureFlag.OGLInformation) )
            {
                reader.BaseStream.Seek(Origin.Current, 1); // 0x01

                reader.BaseStream.Seek(Origin.Current, 1); // 0x01
                
                GraphicsDriver = reader.ReadString();
                
                GraphicsVersion = reader.ReadString();
            }

            if (features.HasFeatureFlag(FeatureFlag.Authenticator) )
            {
                position = reader.BaseStream.Position;

                var stayLoggedIn = reader.ReadBool();

                var length = reader.ReadUShort();

                if (length > 8)
                {
                    // Examples:
                    //
                    //                                  00 FF FB 8B 1F 23 32 FA FF
                    //                                  01 FF FB 8B 1F 23 32 FA FF

                    StayLoggedIn = stayLoggedIn;
                }
                else
                {
                    // Examples:
                    //
                    // 00 08 00 09 09 09 09 09 09 09 09 00 FF FB 8B 1F 23 32 FA FF
                    // 00 08 00 09 09 09 09 09 09 09 09 01 FF FB 8B 1F 23 32 FA FF

                    AuthenticatorCode = reader.ReadString(length);

                    StayLoggedIn = reader.ReadBool();
                }

                reader.BaseStream.Seek(Origin.Current, 128 - (reader.BaseStream.Position - position) ); // RSA Padding
            }
        }
    }
}