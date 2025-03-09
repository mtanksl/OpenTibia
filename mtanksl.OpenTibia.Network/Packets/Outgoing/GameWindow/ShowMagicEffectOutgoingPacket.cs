using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ShowMagicEffectOutgoingPacket : IOutgoingPacket
    {
        public ShowMagicEffectOutgoingPacket(Position position, MagicEffectType magicEffectType)
        {
            this.Position = position;

            this.MagicEffectType = magicEffectType;
        }

        public Position Position { get; set; }

        public MagicEffectType MagicEffectType { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x83 );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            writer.Write( (byte)MagicEffectType );
        }
    }
}