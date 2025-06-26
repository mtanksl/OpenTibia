using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class TalkIncomingPacket : IIncomingPacket
    {
        public MessageMode MessageMode { get; set; }

        public string Name { get; set; }

        public ushort ChannelId { get; set; }

        public string Message { get; set; }
        
        public void Read(IByteArrayStreamReader reader, IHasFeatureFlag features)        
        {
            MessageMode = features.GetMessageModeForByte(reader.ReadByte() );

            switch (MessageMode)
            {                   
                case MessageMode.PrivateTo:
                case MessageMode.GamemasterPrivateTo:
                case MessageMode.RVRAnswer:

                    Name = reader.ReadString();

                    break;

                case MessageMode.Channel:
                case MessageMode.ChannelHighlight:
                case MessageMode.ChannelManagement:
                case MessageMode.GamemasterChannel:
                case MessageMode.GamemasterChannelAnonymous:

                    ChannelId = reader.ReadUShort();

                    break;
            }

            Message = reader.ReadString();
        }      
    }
}