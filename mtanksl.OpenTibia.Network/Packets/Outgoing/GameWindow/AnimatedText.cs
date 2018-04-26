using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class AnimatedText : IOutgoingPacket
    {
        public AnimatedText(Position position, AnimatedTextColor animatedTextColor, string message)
        {
            this.Position = position;

            this.AnimatedTextColor = animatedTextColor;

            this.Message = message;
        }

        public Position Position { get; set; }

        public AnimatedTextColor AnimatedTextColor { get; set; }

        public string Message { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
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