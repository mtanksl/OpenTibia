using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using System;

namespace OpenTibia.Common.Objects
{
    public class PacketToCommand<T> : IPacketToCommand where T : IIncomingPacket
    {
        private Func<T, Command> convert;

        public PacketToCommand(Func<T, Command> convert)
        {
            this.convert = convert;
        }

        public Command Convert(ByteArrayStreamReader reader)
        {
            var packet = Activator.CreateInstance<T>();

            packet.Read(reader);

            return convert(packet);
        }
    }
}