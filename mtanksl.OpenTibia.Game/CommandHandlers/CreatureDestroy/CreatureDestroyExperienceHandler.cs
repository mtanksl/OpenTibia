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
                    ulong totalExperience = (ulong)monster.Metadata.Experience;

                    ulong totalDamage = (ulong)hits.Values.Sum(h => h.Damage);

                    foreach (var pair in hits)
                    {
                        Creature attacker = pair.Key;

                        ulong damage = (ulong)pair.Value.Damage;

                        if (attacker is Player player)
                        {
                            if (attacker.Tile == null || attacker.IsDestroyed)
                            {

                            }
                            else
                            {
                                ulong experience = totalExperience * damage / totalDamage;

                                await Context.AddCommand(new PlayerAddExperienceCommand(player, experience) );
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

                        lossPercent = (50 * x * x * x + 2250 * x * x - 12100 * x + 20000) / player.Experience;
                    }
                    else
                    {
                        lossPercent = 10;
                    }

                    if (player.Vocation == Vocation.EliteKnight || 
                        player.Vocation == Vocation.RoyalPaladin || 
                        player.Vocation == Vocation.ElderDruid || 
                        player.Vocation == Vocation.MasterSorcerer)
                    {
                        lossPercent *= 0.7;
                    }

                    int blesses = player.Blesses.Count;

                    lossPercent *= Math.Pow(0.92, blesses) / 100;

                    ulong experience = (ulong)(player.Experience * lossPercent);

                    if (experience > 0)
                    {
                        ushort correctLevel;

                        byte correctLevelPercent;

                        if (player.Experience > experience)
                        {
                            correctLevel = player.Level;

                            correctLevelPercent = 0;

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
                        }
                        else
                        {
                            correctLevel = 1;

                            correctLevelPercent = 0;
                        }

                        if (correctLevel < player.Level)
                        {
                            Context.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You downgraded from level " + player.Level + " to level " + correctLevel + ".") );
                        
                            var vocationConfig = Context.Server.Vocations.GetVocationById( (byte)player.Vocation);

                            player.Capacity = (uint)(player.Capacity - (player.Level - correctLevel) * vocationConfig.CapacityPerLevel * 100);

                            player.MaxHealth = (ushort)(player.MaxHealth - (player.Level - correctLevel) * vocationConfig.HealthPerLevel);

                            player.MaxMana = (ushort)(player.MaxMana - (player.Level - correctLevel) * vocationConfig.ManaPerLevel);
                        }

                        await Context.AddCommand(new PlayerUpdateExperienceCommand(player, player.Experience - experience, correctLevel, correctLevelPercent) );
                    }
                }
            }
        }
    }
}