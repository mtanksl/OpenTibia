using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class CombatControlsIncomingPacket : IIncomingPacket
    {
        public FightMode FightMode { get; set; }

        public ChaseMode ChaseMode { get; set; }

        public SafeMode SafeMode { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)
        {
            FightMode = (FightMode)reader.ReadByte();

            ChaseMode = (ChaseMode)reader.ReadByte();

            SafeMode = (SafeMode)reader.ReadByte();
        }
    }
}