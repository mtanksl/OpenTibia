using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ShowAnimatedTextOutgoingPacket : IOutgoingPacket
    {
        public ShowAnimatedTextOutgoingPacket(Position position, AnimatedTextColor animatedTextColor, string message)
        {
            this.Position = position;

            this.AnimatedTextColor = animatedTextColor;

            this.Message = message;
        }

        public Position Position { get; set; }

        public AnimatedTextColor AnimatedTextColor { get; set; }

        public string Message { get; set; }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x84 );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);
            
            writer.Write( (byte)AnimatedTextColor );

            writer.Write(Message);
        }
    }
}