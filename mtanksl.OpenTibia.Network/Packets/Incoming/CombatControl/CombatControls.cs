using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CombatControls : IIncomingPacket
    {
        public FightMode FightMode { get; set; }

        public ChaseMode ChaseMode { get; set; }

        public SafeMode SafeMode { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            FightMode = (FightMode)reader.ReadByte();

            ChaseMode = (ChaseMode)reader.ReadByte();

            SafeMode = (SafeMode)reader.ReadByte();
        }
    }
}