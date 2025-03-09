using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class SelectedCharacterIncomingPacket : IIncomingPacket
    {
        public OperatingSystem OperatingSystem { get; set; }

        public ushort Version { get; set; }

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

            Version = reader.ReadUShort();

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
                Account = reader.ReadUInt().ToString();
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