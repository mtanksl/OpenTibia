﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class FluidItemHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            if (command.Item is FluidItem fromItem)
            {
                if (fromItem.FluidType == FluidType.Empty)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ItIsEmpty) );

                    return Promise.Break;
                }

                return Context.AddCommand(new FluidItemUpdateFluidTypeCommand(fromItem, FluidType.Empty) ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.ToCreature, TalkType.MonsterSay, "Gulp.") );
                } );                
            }

            return next();
        }
    }
}