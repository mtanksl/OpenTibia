﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class FishingRodHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private static HashSet<ushort> fishingRods = new HashSet<ushort>() { 2580 };

        private static HashSet<ushort> shallowWaters = new HashSet<ushort>() { 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4820, 4821, 4822, 4823, 4824, 4825 };

        private static ushort worm = 3976;

        private static ushort fish = 2667;

        public override async Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            if (fishingRods.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                if (command.ToItem.Parent is Tile toTile && !Context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                    await Promise.Break;
                }
                else
                {
                    if (shallowWaters.Contains(command.ToItem.Metadata.OpenTibiaId) )
                    {
                        if ( !command.Player.Tile.ProtectionZone)
                        {
                            int count = await Context.AddCommand(new PlayerCountItemsCommand(command.Player, worm, 1) );

                            if (count > 0 && Context.Server.Randomization.HasProbability(1.0 / 10) )
                            {
                                await Context.AddCommand(new PlayerDestroyItemsCommand(command.Player, worm, 1, 1) );

                                await Context.AddCommand(new PlayerCreateItemCommand(command.Player, fish, 1) );

                                await Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.HereFishyFishy, 1000, "Here, Fishy Fishy!") );
                            }
                        }

                        await Context.AddCommand(new ShowMagicEffectCommand(command.ToItem, MagicEffectType.BlueRings) );
                    }
                    else
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );
             
                        await Promise.Break;
                    }
                }
            }
            else
            {
                await next();
            }
        }
    }
}