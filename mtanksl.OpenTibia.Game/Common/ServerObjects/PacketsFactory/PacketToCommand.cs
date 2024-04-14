using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using System;

namespace OpenTibia.Common.Objects
{
    public class PacketToCommand<T> : IPacketToCommand where T : IIncomingPacket, new()
    {
        private Func<IConnection, T, IncomingCommand> convert;

        public PacketToCommand(string name, Func<IConnection, T, IncomingCommand> convert)
        {
            this.name = name;

            this.convert = convert;
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public IncomingCommand Convert(IConnection connection, ByteArrayStreamReader reader)
        {
            var packet = new T();

            packet.Read(reader);

            return convert(connection, packet);
        }
    }
}