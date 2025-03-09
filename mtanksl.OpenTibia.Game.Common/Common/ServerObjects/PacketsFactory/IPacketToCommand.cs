using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.IO;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPacketToCommand
    {
        string Name { get; }

        IncomingCommand Convert(IConnection connection, ByteArrayStreamReader reader, IHasFeatureFlag features);
    }
}