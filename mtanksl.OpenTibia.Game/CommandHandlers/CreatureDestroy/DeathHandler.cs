using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                    uint maxDamage = 0;

                    Player owner = null;

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

                                if (damage > maxDamage)
                                {
                                    owner = player;
                                }

                                ulong experience = totalExperience * damage / totalDamage;

                                if (experience > 0)
                                {
                                    await Context.AddCommand(new PlayerAddExperienceCommand(player, experience) );
                                }
                            }
                        }
                    }

                    var corpse = await Context.AddCommand(new TileCreateMonsterCorpseCommand(command.Creature.Tile, monster.Metadata) );
                    
                             _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromSeconds(30) ) );

                    if (owner != null)
                    {
                        if (corpse is Container container)
                        {
                            StringBuilder builder = new StringBuilder();

                            foreach (var item in container.GetItems() )
                            {
                                builder.Append(item.Metadata.GetDescription(item is StackableItem stackableItem ? stackableItem.Count : (byte)1) + ", ");
                            }

                            string message;

                            if (builder.Length > 2)
                            {
                                builder.Remove(builder.Length - 2, 2);
               
                                message = "Loot of " + monster.Metadata.Description + ": " + builder.ToString() + ".";
                            }
                            else
                            {
                                message = "Loot of " + monster.Metadata.Description + ": nothing.";
                            }

                            Party party = Context.Server.Parties.GetPartyThatContainsMember(owner);

                            if (party != null)
                            {
                                foreach (var member in party.GetMembers() )
                                {
                                    Context.AddPacket(member, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );
                                }
                            }
                            else
                            {
                                Context.AddPacket(owner, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, message) );
                            }
                        }
                    }
                }
                else if (command.Creature is Player player)
                {
                    int blesses = player.Blesses.Count;

                    player.Blesses.ClearBlesses();

                    double lossPercent;

                    if (Context.Server.Config.GameplayDeathLosePercent < 0)
                    {
                        lossPercent = Formula.GetLossPercent(player.Level, player.LevelPercent, player.Experience, player.Vocation, blesses);
                    }
                    else
                    {
                        lossPercent = Context.Server.Config.GameplayDeathLosePercent / 100.0;
                    }

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

                        var corpse = await Context.AddCommand(new TileCreatePlayerCorpseCommand(command.Creature.Tile, player, true, blesses) );
                        
                                 _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromMinutes(5) ) );
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

                        var corpse = await Context.AddCommand(new TileCreatePlayerCorpseCommand(command.Creature.Tile, player, player.Client.GetSkullItem(player) == SkullIcon.Red || player.Client.GetSkullItem(player) == SkullIcon.Black, blesses) );
                        
                                 _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromMinutes(5) ) );
                    }
                }

                Context.AddEvent(command.Creature, new CreatureDeathEventArgs(command.Creature) );
            }
        }
    }
}