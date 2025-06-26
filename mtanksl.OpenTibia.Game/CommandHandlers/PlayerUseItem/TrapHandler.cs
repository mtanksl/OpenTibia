using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class TrapHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> traps;

        public TrapHandler()
        {
            traps = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.traps");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (traps.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is Tile tile && tile.ProtectionZone)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.ThisActionIsNotPermittedInAProtectionZone) );

                    return Promise.Break;
                }

                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next();
        }
    }
}