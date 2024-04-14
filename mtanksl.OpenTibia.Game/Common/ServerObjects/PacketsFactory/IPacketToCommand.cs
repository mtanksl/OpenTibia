using OpenTibia.Game.Commands;
using OpenTibia.IO;

namespace OpenTibia.Common.Objects
{
    public interface IPacketToCommand
    {
        public string Name { get; }

        IncomingCommand Convert(IConnection connection, ByteArrayStreamReader reader);
    }
}