using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SelectedCharacterIncomingPacket : IIncomingPacket
    {
        public OperatingSystem OperatingSystem { get; set; }

        public ushort ProtocolVersion { get; set; }

        public uint ClientVersion { get; set; }

        public ushort ContentRevision { get; set; }

        public uint[] Keys { get; set; }

        public bool Gamemaster { get; set; }

        public string Account { get; set; }

        public string Character { get; set; }

        public string Password { get; set; }

        public uint Timestamp { get; set; }

        public byte Random { get; set; }

        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            OperatingSystem = (OperatingSystem)reader.ReadUShort();

            ProtocolVersion = reader.ReadUShort();
                        
            if (features.HasFeatureFlag(FeatureFlag.ClientVersion) )
            {
                ClientVersion = reader.ReadUInt();
            }
            else
            {
                ClientVersion = ProtocolVersion;
            }

            if (features.HasFeatureFlag(FeatureFlag.ContentRevision) )
            {
                ContentRevision = reader.ReadUShort();
            }

            if (features.HasFeatureFlag(FeatureFlag.PreviewState) )
            {
                reader.BaseStream.Seek(Origin.Current, 1);
            }

            reader.BaseStream.Seek(Origin.Current, 1);

            Keys = new uint[]
            {
                reader.ReadUInt(),

                reader.ReadUInt(),

                reader.ReadUInt(),

                reader.ReadUInt()
            };

            Gamemaster = reader.ReadBool();

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

            Character = reader.ReadString();

            Password = reader.ReadString();

            if (features.HasFeatureFlag(FeatureFlag.ChallengeOnLogin) )
            {
                Timestamp = reader.ReadUInt();

                Random = reader.ReadByte();
            }
        }
    }
}