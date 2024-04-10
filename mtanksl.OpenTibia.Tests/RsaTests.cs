using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.IO;
using OpenTibia.Security;
using System.Globalization;
using System.Linq;

namespace mtanksl.OpenTibia.Tests
{
    [TestClass]
    public class RsaTests
    {
        [TestMethod]
        public void LoginPacketTibia860()
        {
            // Without the first 2 bytes for length

            string print = "B4 46 32 1E 01 02 00 5C 03 93 79 2C 4C 94 05 22 4C D3 C3 E5 4A 47 28 D9 10 7B B7 05 19 D2 2C F5 6B 67 2C CA 02 FD CF AF F8 A4 C5 8A CF 24 AE 1A 63 82 A8 F6 8A 5C 21 9B CF BE 29 8D 8B 33 F0 B8 5C 92 60 BE C2 E2 72 05 3D 52 AC 9A 1B 4F 0D DE 8E AF DA 84 A7 08 A7 43 00 00 61 E7 49 35 A8 F6 4B B4 1E 93 B1 3D E8 DB 53 56 58 E4 2C 0D 31 B6 EA 08 CA 79 EE 4B 98 B7 5F E5 31 5F D6 1B 91 67 CD A1 38 D9 9A D8 04 DA 97 DE 07 82 6D 4E A9 F9 37 FF 93 6E 83";

            byte[] body = print.Split(' ').Select(b => (byte)int.Parse(b, NumberStyles.HexNumber) ).ToArray();

            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            Rsa.DecryptAndReplace(body, 21, 128);

            var adler32 = reader.ReadUInt(); //506611380

            byte identification = reader.ReadByte(); //1

            ushort operatingSystem = reader.ReadUShort(); //2

            ushort version = reader.ReadUShort(); //860

            uint tibiaDat = reader.ReadUInt(); //1277983123

            uint tibiaSpr = reader.ReadUInt(); //1277298068

            uint tibiaPic = reader.ReadUInt(); //1256571859

            reader.BaseStream.Seek(Origin.Current, 1);

            uint[] keys = new uint[]
            {
                reader.ReadUInt(), 
                
                reader.ReadUInt(),
                
                reader.ReadUInt(), 
                
                reader.ReadUInt()
            };

            string account = reader.ReadString(); //hello

            string password = reader.ReadString(); //world

            Assert.AreEqual("hello", account);

            Assert.AreEqual("world", password);
        }

        [TestMethod]
        public void LoginPacketTibia772()
        {
            // Without the first 2 bytes for length

            string print = "01 02 00 04 03 33 5A 9D 43 BE 52 98 43 D8 C8 50 44 01 FF E9 CE CE 98 CB FC CE 4C 59 B3 ED D7 19 5D 23 0F AA B0 0E 89 74 73 F1 1D 7F CE 72 86 04 CC CD B8 18 78 B8 FE 99 DB 49 5B 06 77 46 CA 78 1C 41 F5 B9 29 EE 3C 80 C6 2E 05 56 A7 6C F0 75 CA F8 70 0B 2A 9D 11 04 04 76 3D 49 8A 6B 3F 05 FD 98 C8 7A 0C 2A 84 6E FB E9 31 B9 E1 21 41 3D 0C E2 6F 91 3D A7 85 24 15 F1 90 01 5C 30 71 0C 8B D2 1E 25 E4 DD 16 AE AF 80 B4 DF 3A 6C 88 A7 A5";

            byte[] body = print.Split(' ').Select(b => (byte)int.Parse(b, NumberStyles.HexNumber) ).ToArray();

            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            Rsa.DecryptAndReplace(body, 17, 128);

            byte identification = reader.ReadByte(); //1

            ushort operatingSystem = reader.ReadUShort(); //2

            ushort version = reader.ReadUShort(); //772

            uint tibiaDat = reader.ReadUInt(); //1134385715

            uint tibiaSpr = reader.ReadUInt(); //1134056126

            uint tibiaPic = reader.ReadUInt(); //1146144984

            reader.BaseStream.Seek(Origin.Current, 1);

            uint[] keys = new uint[]
            {
                reader.ReadUInt(),

                reader.ReadUInt(),

                reader.ReadUInt(),

                reader.ReadUInt()
            };

            uint account = reader.ReadUInt(); //123456

            string password = reader.ReadString(); //world

            Assert.AreEqual(123456u, account);

            Assert.AreEqual("world", password);
        }
    }
}