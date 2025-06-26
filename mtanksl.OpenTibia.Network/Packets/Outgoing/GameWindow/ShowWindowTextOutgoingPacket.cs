using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ShowWindowTextOutgoingPacket : IOutgoingPacket
    {
        private int option;

        /// <summary>
        /// Console message.
        /// <br/> 
        /// <br/> MessageMode.Failure
        /// <br/> MessageMode.Game
        /// <br/> MessageMode.HotkeyUse
        /// <br/> MessageMode.Login
        /// <br/> MessageMode.Look
        /// <br/> MessageMode.Loot
        /// <br/> MessageMode.Market
        /// <br/> MessageMode.Report
        /// <br/> MessageMode.Status
        /// <br/> MessageMode.Thankyou
        /// <br/> MessageMode.TradeNpc
        /// <br/> MessageMode.Warning
        /// </summary>
        public ShowWindowTextOutgoingPacket(MessageMode messageMode, string message)
        {
            this.option = 1;


            this.MessageMode = messageMode;

            this.Message = message;
        }

        /// <summary>
        /// Animated message.
        /// <br/>
        /// <br/> MessageMode.Heal
        /// <br/> MessageMode.Mana
        /// <br/> MessageMode.Exp
        /// <br/> MessageMode.HealOthers
        /// <br/> MessageMode.ExpOthers
        /// </summary>
        public ShowWindowTextOutgoingPacket(MessageMode messageMode, Position position, uint value, AnimatedTextColor color, string message)
        {
            this.option = 2;


            this.MessageMode = messageMode;

            this.Position = position;

            this.Value = value;

            this.Color = color;

            this.Message = message;
        }

        /// <summary>
        /// Animated message.
        /// <br/>
        /// <br/> MessageMode.DamageDealed
        /// <br/> MessageMode.DamageReceived
        /// <br/> MessageMode.DamageOthers
        /// </summary>
        public ShowWindowTextOutgoingPacket(MessageMode messageMode, Position position, uint physicalDamageValue, AnimatedTextColor physicalDamageColor, uint magicalDamageValue, AnimatedTextColor magicalDamageColor, string message)
        {
            this.option = 3;


            this.MessageMode = messageMode;

            this.Position = position;

            this.Value = physicalDamageValue;

            this.Color = physicalDamageColor;

            this.Value2 = magicalDamageValue;

            this.Color2 = magicalDamageColor;

            this.Message = message;
        }

        /// <summary>
        /// Channel message.
        /// <br/>
        /// <br/> MessageMode.ChannelManagement
        /// <br/> MessageMode.Guild
        /// <br/> MessageMode.PartyManagement
        /// <br/> MessageMode.Party
        /// </summary>
        public ShowWindowTextOutgoingPacket(MessageMode messageMode, ushort channelId, string message)
        {
            this.option = 4;


            this.MessageMode = messageMode;

            this.ChannelId = channelId;

            this.Message = message;
        }

        public MessageMode MessageMode { get; set; }

        public Position Position { get; set; }

        public uint Value { get; set; }

        public AnimatedTextColor Color { get; set; }

        public uint Value2 { get; set; }

        public AnimatedTextColor Color2 { get; set; }

        public ushort ChannelId { get; set; }

        public string Message { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xB4 );

            writer.Write(features.GetByteForMessageMode(MessageMode) );

            switch (option)
            {
                case 1:

                    writer.Write(Message);

                    break;

                case 2:

                    writer.Write(Position.X);

                    writer.Write(Position.Y);

                    writer.Write(Position.Z);

                    writer.Write(Value);

                    writer.Write( (byte)Color);

                    writer.Write(Message);

                    break;

                case 3:

                    writer.Write(Position.X);

                    writer.Write(Position.Y);

                    writer.Write(Position.Z);

                    writer.Write(Value);

                    writer.Write( (byte)Color);

                    writer.Write(Value2);

                    writer.Write( (byte)Color2);

                    writer.Write(Message);

                    break;

                case 4:

                    writer.Write(ChannelId);

                    writer.Write(Message);

                    break;
            }
        }
    }
}