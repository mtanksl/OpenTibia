using OpenTibia.IO;

namespace OpenTibia
{
    public class MagicEffectOutgoingPacket : IOutgoingPacket
    {
        public MagicEffectOutgoingPacket(Position position, MagicEffectType magicEffectType)
        {
            this.Position = position;

            this.MagicEffectType = magicEffectType;
        }

        public Position Position { get; set; }

        public MagicEffectType MagicEffectType { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x83 );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            writer.Write( (byte)MagicEffectType );
        }
    }
}