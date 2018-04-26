using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class StopWalk : IOutgoingPacket
    {
        public StopWalk(Direction direction)
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