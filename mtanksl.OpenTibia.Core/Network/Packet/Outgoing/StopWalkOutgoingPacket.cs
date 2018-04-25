using OpenTibia.IO;

namespace OpenTibia
{
    public class StopWalkOutgoingPacket : IOutgoingPacket
    {
        public StopWalkOutgoingPacket(Direction direction)
        {
            this.Direction = direction;
        }

        public Direction Direction { get; set; }
                
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xB5 );

            writer.Write( (byte)Direction );
        }
    }
}