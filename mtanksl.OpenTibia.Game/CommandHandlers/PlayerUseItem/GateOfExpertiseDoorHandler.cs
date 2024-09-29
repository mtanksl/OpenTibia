using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class GateOfExpertiseDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> openGateOfExpertiseDoors;

        public GateOfExpertiseDoorHandler()
        {
            openGateOfExpertiseDoors = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.openGateOfExpertiseDoors");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (openGateOfExpertiseDoors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.ActionId < 1000 || command.Player.Level < command.Item.ActionId - 1000)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "Only the worthy may pass.") );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    return Context.AddCommand(new CreatureMoveCommand(command.Player, (Tile)item.Parent) );
                } );
            }

            return next();
        }
    }
}