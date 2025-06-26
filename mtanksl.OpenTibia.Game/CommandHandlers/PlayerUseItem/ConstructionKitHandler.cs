using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ConstructionKitHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> constructionKits;

        public ConstructionKitHandler()
        {
            constructionKits = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.constructionKits");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (constructionKits.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is Tile)
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.Puff) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                    } );
                }

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.PutTheContructionKitOnTheFloorFirst) );

                return Promise.Break;
            }

            return next();
        }
    }
}