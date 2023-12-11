using OpenTibia.Game.Commands;
using OpenTibia.IO;

namespace OpenTibia.Common.Objects
{
    public interface IPacketToCommand
    {
        Command Convert(ByteArrayStreamReader reader);
    }
}