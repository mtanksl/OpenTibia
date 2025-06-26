using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SealedDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> openSealedDoors;

        public SealedDoorHandler()
        {
            openSealedDoors = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.openSealedDoors");
        }
        
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (openSealedDoors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.ActionId < 1000 || !command.Player.Storages.TryGetValue(command.Item.ActionId - 1000, out _) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, "The door seems to be sealed against unwanted intruders.") );

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