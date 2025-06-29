using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ShowTextOutgoingPacket : IOutgoingPacket
    {
        private int option;

        /// <summary>
        /// Private message.
        /// <br/>
        /// <br/> MessageMode.PrivateFrom
        /// <br/> MessageMode.GamemasterBroadcast
        /// <br/> MessageMode.GamemasterPrivateFrom
        /// <br/> MessageMode.RVRAnswer
        /// <br/> MessageMode.RVRContinue
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, MessageMode messageMode, string message)
        {
            this.option = 1;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.MessageMode = messageMode;

            this.Message = message;
        }

        /// <summary>
        /// Public message.
        /// <br/>
        /// <br/> MessageMode.Say
        /// <br/> MessageMode.Whisper
        /// <br/> MessageMode.Yell
        /// <br/> MessageMode.MonsterSay
        /// <br/> MessageMode.MonsterYell
        /// <br/> MessageMode.NpcTo
        /// <br/> MessageMode.BarkLow
        /// <br/> MessageMode.BarkLoud
        /// <br/> MessageMode.Spell
        /// <br/> MessageMode.NpcFromStartBlock
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, MessageMode messageMode, Position position, string message)
        {
            this.option = 2;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.MessageMode = messageMode;

            this.Position = position;

            this.Message = message;
        }

        /// <summary>
        /// Channel message.
        /// <br/>
        /// <br/> MessageMode.Channel
        /// <br/> MessageMode.ChannelManagement
        /// <br/> MessageMode.ChannelHighlight
        /// <br/> MessageMode.GamemasterChannel
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, MessageMode messageMode, ushort channelId, string message)
        {
            this.option = 3;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.MessageMode = messageMode;

            this.ChannelId = channelId;

            this.Message = message;
        }

        /// <summary>
        /// Rule violation message.
        /// <br/>
        /// <br/> MessageMode.RVRChannel
        /// </summary>
        public ShowTextOutgoingPacket(uint statmentId, string sender, ushort level, uint time, string message)
        {
            this.option = 4;


            this.StatmentId = statmentId;

            this.Sender = sender;

            this.Level = level;

            this.MessageMode = MessageMode.RVRChannel;

            this.Time = time;

            this.Message = message;
        }

        public uint StatmentId { get; set; }

        public string Sender { get; set; }

        public ushort Level { get; set; }

        public MessageMode MessageMode { get; set; }

        public Position Position { get; set; }

        public ushort ChannelId { get; set; }

        public uint Time { get; set; }

        public string Message { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xAA );

            if (features.HasFeatureFlag(FeatureFlag.MessageStatement) )
            {
                writer.Write(StatmentId);
            }

            writer.Write(Sender);

            if (features.HasFeatureFlag(FeatureFlag.MessageLevel) )
            {
                writer.Write(Level);
            }

            writer.Write(features.GetByteForMessageMode(MessageMode) );

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