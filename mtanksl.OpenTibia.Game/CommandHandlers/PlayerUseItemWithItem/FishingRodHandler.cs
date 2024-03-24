using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FishingRodHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private HashSet<ushort> fishingRods = new HashSet<ushort>() { 2580 };

        private HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4820, 4821, 4822, 4823, 4824, 4825 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (fishingRods.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (shallowWaters.Contains(command.ToItem.Metadata.OpenTibiaId) )
                {
                    return Context.AddCommand(new ShowMagicEffectCommand( ( (Tile)command.ToItem.Parent).Position, MagicEffectType.BlueRings) );
                }

                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                return Promise.Break;
            }

            return next();
        }
    }
}