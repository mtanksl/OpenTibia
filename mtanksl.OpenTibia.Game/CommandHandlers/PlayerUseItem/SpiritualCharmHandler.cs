﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpiritualCharmHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> spiritualCharms;
        
        public SpiritualCharmHandler()
        {
            spiritualCharms = Context.Server.Values.GetUInt16HashSet("values.items.spiritualCharms");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (spiritualCharms.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerBlessCommand(command.Player, "The Spiritual Shielding protects you.", "Spiritual Charm") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.BlueRings) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}