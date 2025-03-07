using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IFeatures
    {
        void Start();

        Dictionary<byte, IPacketToCommand> LoginFirstCommands { get; }

        Dictionary<byte, IPacketToCommand> GameFirstCommands { get; }

        Dictionary<byte, IPacketToCommand> GameCommands { get; }

        Dictionary<byte, IPacketToCommand> GameAccountManagerCommands { get; }

		Dictionary<byte, IPacketToCommand> InfoFirstCommands { get; }
    }
}