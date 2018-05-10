using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class Login : IIncomingPacket
    {
        public OperatingSystem OperatingSystem { get; set; }

        public ushort Version { get; set; }

        public uint[] Keys { get; set; }

        public bool Gamemaster { get; set; }

        public string Account { get; set; }

        public string Character { get; set; }

        public string Password { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            OperatingSystem = (OperatingSystem)reader.ReadUShort();

            Version = reader.ReadUShort();

            reader.ReadByte();

            Keys = new uint[]
            {
                reader.ReadUInt(),

                reader.ReadUInt(),

                reader.ReadUInt(),

                reader.ReadUInt()
            };

            Gamemaster = reader.ReadBool();

            Account = reader.ReadString();

            Character = reader.ReadString();

            Password = reader.ReadString();
        }
    }
}