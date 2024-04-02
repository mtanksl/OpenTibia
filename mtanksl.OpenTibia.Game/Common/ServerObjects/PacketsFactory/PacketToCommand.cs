using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using System;

namespace OpenTibia.Common.Objects
{
    public class PacketToCommand<T> : IPacketToCommand where T : IIncomingPacket, new()
    {
        private Func<IConnection, T, Command> convert;

        public PacketToCommand(Func<IConnection, T, Command> convert)
        {
            this.convert = convert;
        }

        public Command Convert(IConnection connection, ByteArrayStreamReader reader)
        {
            var packet = new T();

            packet.Read(reader);

            return convert(connection, packet);
        }
    }
}