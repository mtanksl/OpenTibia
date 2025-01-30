using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkToIncomingPacket : IIncomingPacket
    {
        public MoveDirection[] MoveDirections { get; set; }
        
        public void Read(IByteArrayStreamReader reader)
        {
            MoveDirections = new MoveDirection[ reader.ReadByte() ];
            
            for (int i = 0; i < MoveDirections.Length; i++)
            {
                MoveDirections[i] = (MoveDirection)reader.ReadByte();
            }
        }
    }
}