﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PiggyBankHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly Dictionary<ushort, ushort> piggyBanks;
        private readonly ushort platinumCoin;
        private readonly ushort goldCoin;

        public PiggyBankHandler()
        {
            piggyBanks = Context.Server.Values.GetUInt16IUnt16Dictionary("values.items.transformation.piggyBanks");
            platinumCoin = Context.Server.Values.GetUInt16("values.items.platinumCoin");
            goldCoin = Context.Server.Values.GetUInt16("values.items.goldCoin");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (piggyBanks.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (Context.Server.Randomization.HasProbability(1.0 / 5) )
                {
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.AllowanceCollector, 50, "Allowance Collector") ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.YellowSpark) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, goldCoin, 1) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                    } );
                }
                else
                {
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.AllowanceCollector, 50, "Allowance Collector") ).Then( () =>
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(command.Item, MagicEffectType.YellowNotes) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, platinumCoin, 1) );
                    } );
                }
            }

            return next();
        }
    }
}