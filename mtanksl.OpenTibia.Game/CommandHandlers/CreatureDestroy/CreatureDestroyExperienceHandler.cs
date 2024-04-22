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
                    uint totalExperience = (uint)Context.Server.Config.GameplayExperienceRate * monster.Metadata.Experience;

                    uint totalDamage = (uint)hits.Values.Sum(h => h.Damage);

                    foreach (var pair in hits)
                    {
                        Creature attacker = pair.Key;

                        uint damage = (uint)pair.Value.Damage;

                        if (attacker.Tile == null || attacker.IsDestroyed )
                        {

                        }
                        else
                        {
                            uint experience = totalExperience * damage / totalDamage;

                            if (experience > 0)
                            {
                                if (attacker is Player player)
                                {
                                    await Context.Current.AddCommand(new ShowAnimatedTextCommand(player, AnimatedTextColor.White, experience.ToString() ) );

                                    ushort correctLevel = player.Level;

                                    byte correctLevelPercent = 0;

                                    uint levelMinExperience = (uint)( ( 50 * Math.Pow(correctLevel - 1, 3) - 150 * Math.Pow(correctLevel - 1, 2) + 400 * (correctLevel - 1) ) / 3 );

                                    while (true)
                                    {
                                        uint levelMaxExperience = (uint)( ( 50 * Math.Pow(correctLevel, 3) - 150 * Math.Pow(correctLevel, 2) + 400 * (correctLevel) ) / 3 );

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
            }
        }
    }
}