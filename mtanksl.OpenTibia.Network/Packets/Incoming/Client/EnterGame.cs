using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class EnterGame : IIncomingPacket
    {
        public OperatingSystem OperatingSystem { get; set; }

        public ushort Version { get; set; }

        public uint TibiaDat { get; set; }

        public uint TibiaSpr { get; set; }

        public uint TibiaPic { get; set; }

        public uint[] Keys { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            OperatingSystem = (OperatingSystem)reader.ReadUShort();

            Version = reader.ReadUShort();

            TibiaDat = reader.ReadUInt();

            TibiaSpr = reader.ReadUInt();

            TibiaPic = reader.ReadUInt();

            reader.ReadByte();

            Keys = new uint[]
            {
                reader.ReadUInt(), 
                
                reader.ReadUInt(),
                
                reader.ReadUInt(), 
                
                reader.ReadUInt()
            };

            Account = reader.ReadString();

            Password = reader.ReadString();
        }
    }
}