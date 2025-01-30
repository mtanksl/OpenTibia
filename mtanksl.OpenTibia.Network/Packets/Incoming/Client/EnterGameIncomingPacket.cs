using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class EnterGameIncomingPacket : IIncomingPacket
    {
        public OperatingSystem OperatingSystem { get; set; }

        public ushort Version { get; set; }

        public uint TibiaDat { get; set; }

        public uint TibiaSpr { get; set; }

        public uint TibiaPic { get; set; }

        public uint[] Keys { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public byte LocaleId { get; set; }

        public string Locate { get; set; }

        public ushort TotalRam { get; set; }

        public string Cpu { get; set; }

        public ushort CpuClock { get; set; }

        public ushort CpuClock2 { get; set; }

        public string Gpu { get; set; }

        public ushort VideoRam { get; set; }

        public ushort ResolutionHorizontal { get; set; }

        public ushort ResolutionVertical { get; set; }

        public byte RefreshRate { get; set; }

        public void Read(IByteArrayStreamReader reader)
        {
            OperatingSystem = (OperatingSystem)reader.ReadUShort();

            Version = reader.ReadUShort();

            TibiaDat = reader.ReadUInt();

            TibiaSpr = reader.ReadUInt();

            TibiaPic = reader.ReadUInt();

            reader.BaseStream.Seek(Origin.Current, 1);

            Keys = new uint[]
            {
                reader.ReadUInt(), 
                
                reader.ReadUInt(),
                
                reader.ReadUInt(), 
                
                reader.ReadUInt()
            };

            Account = reader.ReadString();

            Password = reader.ReadString();

            LocaleId = reader.ReadByte();

            Locate = reader.ReadString(3);

            TotalRam = reader.ReadUShort();

            reader.BaseStream.Seek(Origin.Current, 6);

            Cpu = reader.ReadString(9);

            reader.BaseStream.Seek(Origin.Current, 2);

            CpuClock = reader.ReadUShort();

            CpuClock2 = reader.ReadUShort();

            reader.BaseStream.Seek(Origin.Current, 4);

            Cpu = reader.ReadString(9);

            VideoRam = reader.ReadUShort();

            ResolutionHorizontal = reader.ReadUShort();

            ResolutionVertical = reader.ReadUShort();

            RefreshRate = reader.ReadByte();
        }
    }
}