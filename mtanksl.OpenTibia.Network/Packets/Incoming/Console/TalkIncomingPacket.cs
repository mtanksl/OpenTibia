using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class TalkIncomingPacket : IIncomingPacket
    {
        public TalkType TalkType { get; set; }

        public string Name { get; set; }

        public ushort ChannelId { get; set; }

        public string Message { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)        
        {
            TalkType = (TalkType)reader.ReadByte();

            switch (TalkType)
            {                   
                case TalkType.Private:

                case TalkType.PrivateRed:

                case TalkType.ReportRuleViolationAnswer:

                    Name = reader.ReadString();

                    break;

                case TalkType.ChannelYellow:

                case TalkType.ChannelWhite:

                case TalkType.ChannelRed:

                case TalkType.ChannelOrange:

                case TalkType.ChannelRedAnonymous:

                    ChannelId = reader.ReadUShort();

                    break;
            }

            Message = reader.ReadString();
        }      
    }
}