using OpenTibia.Game.Commands;
using OpenTibia.IO;

namespace OpenTibia.Common.Objects
{
    public interface IPacketToCommand
    {
        IncomingCommand Convert(IConnection connection, ByteArrayStreamReader reader);
    }
}