﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SolitudeCharmHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> solitudeCharms;
        
        public SolitudeCharmHandler()
        {
            solitudeCharms = Context.Server.Values.GetUInt16HashSet("values.items.solitudeCharms");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (solitudeCharms.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerBlessCommand(command.Player, "The Wisdom of Solitude inspires you.", "Solitude Charm") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.GreenShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}