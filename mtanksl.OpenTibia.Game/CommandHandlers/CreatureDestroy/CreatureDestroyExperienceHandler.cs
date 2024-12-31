using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreatureDestroyExperienceHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override async Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            await next();

            Dictionary<Creature, Hit> hits = Context.Server.Combats.GetHitsByTargetAndRemove(command.Creature);
            
            if (command.Creature.Health == 0)
            {
                if (command.Creature is Monster monster)
                {
                    ulong totalExperience = (ulong)Context.Server.Config.GameplayExperienceRate * monster.Metadata.Experience;

                    ulong totalDamage = (ulong)hits.Values.Sum(h => h.Damage);

                    foreach (var pair in hits)
                    {
                        Creature attacker = pair.Key;

                        ulong damage = (ulong)pair.Value.Damage;

                        if (attacker.Tile == null || attacker.IsDestroyed)
                        {

                        }
                        else
                        {
                            ulong experience = totalExperience * damage / totalDamage;

                            if (experience > 0)
                            {
                                if (attacker is Player player)
                                {
                                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(player, AnimatedTextColor.White, experience.ToString() ) );

                                    ushort correctLevel = player.Level;

                                    byte correctLevelPercent = 0;

                                    ulong levelMinExperience = (ulong)( ( 50 * Math.Pow(correctLevel - 1, 3) - 150 * Math.Pow(correctLevel - 1, 2) + 400 * (correctLevel - 1) ) / 3 );

                                    while (true)
                                    {
                                        ulong levelMaxExperience = (ulong)( ( 50 * Math.Pow(correctLevel, 3) - 150 * Math.Pow(correctLevel, 2) + 400 * (correctLevel) ) / 3 );

                                        if (player.Experience + experience < levelMaxExperience)
                                        {
                                            correctLevelPercent = (byte)( 100 * (player.Experience + experience - levelMinExperience) / (levelMaxExperience - levelMinExperience) );

                                            break;
                                        }
                                        else
                                        {
                                            correctLevel++;

                                            levelMinExperience = levelMaxExperience;
                                        }
                                    }

                                    if (correctLevel > player.Level)
                                    {
                                        Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You advanced from level " + player.Level + " to level " + correctLevel + ".") );
                                    }

                                    await Context.Current.AddCommand(new PlayerUpdateExperienceCommand(player, player.Experience + experience, correctLevel, correctLevelPercent) );
                                }
                            }
                        }
                    }
                }
                else if (command.Creature is Player player)
                {
                    double lossPercent;

                    if (player.Level >= 24)
                    {
                        double x = player.Level + player.LevelPercent / 100.0;

                        lossPercent = (0.5 * x * x * x + 22.5 * x * x - 121 * x + 200) / player.Experience;
                    }
                    else
                    {
                        lossPercent = 0.1;
                    }

                    if (player.Vocation == Vocation.EliteKnight || player.Vocation == Vocation.RoyalPaladin || player.Vocation == Vocation.ElderDruid || player.Vocation == Vocation.MasterSorcerer)
                    {
                        lossPercent *= 0.7;
                    }

                    ulong experience = (ulong)(player.Experience * lossPercent);

                    if (experience > 0)
                    {
                        if (player.Experience > experience)
                        {
                            ushort correctLevel = player.Level;

                            byte correctLevelPercent = 0;

                            ulong levelMaxExperience = (ulong)( ( 50 * Math.Pow(correctLevel, 3) - 150 * Math.Pow(correctLevel, 2) + 400 * (correctLevel) ) / 3 );

                            while (true)
                            {
                                ulong levelMinExperience = (ulong)( ( 50 * Math.Pow(correctLevel - 1, 3) - 150 * Math.Pow(correctLevel - 1, 2) + 400 * (correctLevel - 1) ) / 3 );

                                if (player.Experience - experience > levelMinExperience)
                                {
                                    correctLevelPercent = (byte)( 100 * (player.Experience - experience - levelMinExperience) / (levelMaxExperience - levelMinExperience) );

                                    break;
                                }
                                else
                                {
                                    correctLevel--;

                                    levelMaxExperience = levelMinExperience;
                                }
                            }

                            if (correctLevel < player.Level)
                            {
                                Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You downgraded from level " + player.Level + " to level " + correctLevel + ".") );
                            }

                            await Context.Current.AddCommand(new PlayerUpdateExperienceCommand(player, player.Experience - experience, correctLevel, correctLevelPercent) );
                        }
                        else
                        {
                            ushort correctLevel = 1;

                            byte correctLevelPercent = 0;

                            if (correctLevel < player.Level)
                            {
                                Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You downgraded from level " + player.Level + " to level " + correctLevel + ".") );
                            }

                            await Context.Current.AddCommand(new PlayerUpdateExperienceCommand(player, player.Experience - experience, correctLevel, correctLevelPercent) );
                        }
                    }
                }
            }
        }
    }
}