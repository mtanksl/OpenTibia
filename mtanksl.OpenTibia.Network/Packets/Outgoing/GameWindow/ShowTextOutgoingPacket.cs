using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ShowTextOutgoingPacket : IOutgoingPacket
    {
        private int option;

        /// <summary>
        /// Private message.
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, TalkType talkType, string message)
        {
            this.option = 1;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.TalkType = talkType;

            this.Message = message;
        }

        /// <summary>
        /// Public message.
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, TalkType talkType, Position position, string message)
        {
            this.option = 2;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.TalkType = talkType;

            this.Position = position;

            this.Message = message;
        }

        /// <summary>
        /// Channel message.
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, TalkType talkType, ushort channelId, string message)
        {
            this.option = 3;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.TalkType = talkType;

            this.ChannelId = channelId;

            this.Message = message;
        }

        /// <summary>
        /// Rule violation message.
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, TalkType talkType, uint time, string message)
        {
            this.option = 4;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.TalkType = talkType;

            this.Time = time;

            this.Message = message;
        }

        public uint StatmentId { get; set; }

        public string Sender { get; set; }

        public ushort Level { get; set; }

        public TalkType TalkType { get; set; }

        public Position Position { get; set; }

        public ushort ChannelId { get; set; }

        public uint Time { get; set; }

        public string Message { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xAA );

            writer.Write(StatmentId);

            writer.Write(Sender);

            writer.Write(Level);

            writer.Write( (byte)TalkType );

            switch (option)
            {
                case 1:

                    break;

                case 2:

                    writer.Write(Position.X);

                    writer.Write(Position.Y);

                    writer.Write(Position.Z);

                    break;

                case 3:

                    writer.Write(ChannelId);

                    break;

                case 4:

                    writer.Write(Time);

                    break;
            }

            writer.Write(Message);
        }
    }
}