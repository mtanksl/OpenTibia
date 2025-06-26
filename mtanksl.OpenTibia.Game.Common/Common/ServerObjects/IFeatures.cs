using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IFeatures : IHasFeatureFlag
    {
        void Start();

        int ClientVersion { get; }

        uint TibiaDat { get; }

        uint TibiaPic { get; }

        uint TibiaSpr { get; }

        Dictionary<byte, IPacketToCommand> LoginFirstCommands { get; }

        Dictionary<byte, IPacketToCommand> GameFirstCommands { get; }

        Dictionary<byte, IPacketToCommand> GameCommands { get; }

        Dictionary<byte, IPacketToCommand> GameAccountManagerCommands { get; }

		Dictionary<byte, IPacketToCommand> InfoFirstCommands { get; }
    }
}