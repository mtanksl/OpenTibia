using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class DeathHandler : CommandHandler<CreatureDestroyCommand>
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

                        if (attacker is Player player)
                        {
                            if (attacker.Tile == null || attacker.IsDestroyed)
                            {

                            }
                            else
                            {
                                ulong damage = (ulong)pair.Value.Damage;

                                ulong experience = totalExperience * damage / totalDamage;

                                if (experience > 0)
                                {
                                    await Context.AddCommand(new PlayerAddExperienceCommand(player, experience) );
                                }
                            }
                        }
                    }

                    await Context.AddCommand(new TileCreateMonsterCorpseCommand(command.Creature.Tile, monster.Metadata) ).Then( (item) =>
                    {                                    
                        _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(30) ) );

                        return Promise.Completed;
                    } );
                }
                else if (command.Creature is Player player)
                {
                    int blesses = player.Blesses.Count;

                    player.Blesses.ClearBlesses();

                    double lossPercent = Formula.GetLossPercent(player.Level, player.LevelPercent, player.Experience, player.Vocation, blesses);
                                    
                    ulong experience = (ulong)(player.Experience * lossPercent);

                    bool rooking = false;

                    if (Context.Server.Config.GameplayRooking.Enabled)
                    {
                        if (player.Vocation != Vocation.None)
                        {
                            if (player.Experience > experience)
                            {
                                if (player.Experience - experience < Context.Server.Config.GameplayRooking.ExperienceThreshold)
                                {
                                    rooking = true;
                                }
                            }
                            else
                            {
                                rooking = true;
                            }
                        }
                    }

                    if (rooking)
                    {
                        await Context.AddCommand(new PlayerRookingCommand(player) );

                        //TODO: Reset player status to level 1

                        //TODO: Reset player skills

                        await Context.AddCommand(new TileCreatePlayerCorpseCommand(command.Creature.Tile, player, true, blesses) ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(30) ) );

                            return Promise.Completed;
                        } );
                    }
                    else
                    {
                        if (experience > 0)
                        {
                            await Context.AddCommand(new PlayerRemoveExperienceCommand(player, experience) );
                        }

                        foreach (var skill in new[] { Skill.MagicLevel, Skill.Fist, Skill.Club, Skill.Sword, Skill.Axe, Skill.Distance, Skill.Shield, Skill.Fish } )
                        {
                            ulong skillPoints = (ulong)(player.Skills.GetSkillPoints(skill) * lossPercent);

                            if (skillPoints > 0)
                            {
                                await Context.AddCommand(new PlayerRemoveSkillPointsCommand(player, skill, skillPoints) );
                            }
                        }
                    

                        await Context.AddCommand(new TileCreatePlayerCorpseCommand(command.Creature.Tile, player, player.SkullIcon == SkullIcon.Red || player.SkullIcon == SkullIcon.Black, blesses) ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(30) ) );

                            return Promise.Completed;
                        } );
                    }
                }

                Context.AddEvent(new CreatureDeathEventArgs(command.Creature.Tile, command.Creature) );
            }
        }
    }
}