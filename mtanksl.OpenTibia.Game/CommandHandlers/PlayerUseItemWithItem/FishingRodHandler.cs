using OpenTibia.Common.Objects;
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
        private readonly HashSet<ushort> fishingRods;
        private readonly HashSet<ushort> shallowWaters;
        private readonly ushort worm;
        private readonly ushort fish;

        public FishingRodHandler()
        {
            fishingRods = Context.Server.Values.GetUInt16HashSet("values.items.fishingRods");
            shallowWaters = Context.Server.Values.GetUInt16HashSet("values.items.shallowWaters");
            worm = Context.Server.Values.GetUInt16("values.items.worm");
            fish = Context.Server.Values.GetUInt16("values.items.fish");
        }

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

                            if (count > 0)
                            {
                                await Context.AddCommand(new PlayerAddSkillPointsCommand(command.Player, Skill.Fish, 1) );

                                if (Context.Server.Randomization.HasProbability(1.0 / 10) )
                                {
                                    await Context.AddCommand(new PlayerDestroyItemsCommand(command.Player, worm, 1, 1) );

                                    await Context.AddCommand(new PlayerCreateItemCommand(command.Player, fish, 1) );

                                    await Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.HereFishyFishy, 1000, "Here, Fishy Fishy!") );
                                }
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