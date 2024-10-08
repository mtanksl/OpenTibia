﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UnityCharmHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> unityCharms;

        public UnityCharmHandler()
        {
            unityCharms = Context.Server.Values.GetUInt16HashSet("values.items.unityCharms");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (unityCharms.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Context.AddCommand(new PlayerBlessCommand(command.Player, "The Embrace of Tibia surrounds you.", "Unity Charm") ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.BlueShimmer) );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                } );
            }

            return next();
        }
    }
}