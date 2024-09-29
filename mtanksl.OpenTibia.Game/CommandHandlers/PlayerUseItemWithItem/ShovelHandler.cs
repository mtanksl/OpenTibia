﻿using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ShovelHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private readonly HashSet<ushort> shovels;
        private readonly Dictionary<ushort, ushort> stonePiles;
        private readonly Dictionary<ushort, ushort> decay;

        public ShovelHandler()
        {
            shovels = Context.Server.Values.GetUInt16HashSet("values.items.shovels");
            stonePiles = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.stonePiles");
            decay = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.decay.stonePiles");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            ushort toOpenTibiaId;

            if (shovels.Contains(command.Item.Metadata.OpenTibiaId) && stonePiles.TryGetValue(command.ToItem.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.TheUndertaker, 500, "The Undertaker") ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.ToItem, toOpenTibiaId, 1) );

                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(10), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}