using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class WalkTo : IIncomingPacket
    {
        public MoveDirection[] MoveDirections { get; set; }
        
        public void Read(ByteArrayStreamReader reader)
        {
            MoveDirections = new MoveDirection[ reader.ReadByte() ];
            
            for (int i = 0; i < MoveDirections.Length; i++)
            {
                MoveDirections[i] = (MoveDirection)reader.ReadByte();
            }
        }
    }
}