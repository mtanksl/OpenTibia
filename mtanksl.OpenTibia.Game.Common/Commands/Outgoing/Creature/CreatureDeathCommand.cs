using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTibia.Game.Commands
{
    public class CreatureDeathCommand : Command
    {
        public CreatureDeathCommand(Creature creature)
        {
            Creature = creature;
        }

        public Creature Creature { get; set; }

        public override async Promise Execute()
        {
            Dictionary<Creature, Hit> hits = Context.Server.Combats.GetHitsByTargetAndRemove(Creature);

            if (Creature is Monster targetMonster)
            {
                ulong totalExperience = (ulong)targetMonster.Metadata.Experience;

                ulong totalDamage = (ulong)hits.Values.Sum(h => h.Damage);

                uint maxDamage = 0;

                Player owner = null;

                foreach (var pair in hits)
                {
                    Creature attacker = pair.Key;

                    if (attacker is Player attackerPlayer)
                    {
                        if (attacker.Tile == null || attacker.IsDestroyed)
                        {

                        }
                        else
                        {
                            ulong damage = (ulong)pair.Value.Damage;

                            if (damage > maxDamage)
                            {
                                owner = attackerPlayer;
                            }

                            ulong experience = totalExperience * damage / totalDamage;

                            if (experience > 0)
                            {
                                await Context.AddCommand(new PlayerAddExperienceCommand(attackerPlayer, experience) );
                            }
                        }
                    }
                }

                var corpse = await Context.AddCommand(new TileCreateMonsterCorpseCommand(Creature.Tile, targetMonster.Metadata) );
                    
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
               
                            message = "Loot of " + targetMonster.Metadata.Description + ": " + builder.ToString() + ".";
                        }
                        else
                        {
                            message = "Loot of " + targetMonster.Metadata.Description + ": nothing.";
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
            else if (Creature is Player targetPlayer)
            {
                int blesses = targetPlayer.Blesses.Count;

                targetPlayer.Blesses.ClearBlesses();

                double lossPercent;

                if (Context.Server.Config.GameplayDeathLosePercent < 0)
                {
                    lossPercent = Formula.GetLossPercent(targetPlayer.Level, targetPlayer.LevelPercent, targetPlayer.Experience, targetPlayer.Vocation, blesses);
                }
                else
                {
                    lossPercent = Context.Server.Config.GameplayDeathLosePercent / 100.0;
                }

                ulong experience = (ulong)(targetPlayer.Experience * lossPercent);

                bool rooking = false;

                if (Context.Server.Config.GameplayRooking.Enabled)
                {
                    if (targetPlayer.Vocation != Vocation.None)
                    {
                        if (targetPlayer.Experience > experience)
                        {
                            if (targetPlayer.Experience - experience < Context.Server.Config.GameplayRooking.ExperienceThreshold)
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
                    await Context.AddCommand(new PlayerRookingCommand(targetPlayer) );

                    var corpse = await Context.AddCommand(new TileCreatePlayerCorpseCommand(Creature.Tile, targetPlayer, true, blesses) );
                        
                             _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromMinutes(5) ) );
                }
                else
                {
                    if (experience > 0)
                    {
                        await Context.AddCommand(new PlayerRemoveExperienceCommand(targetPlayer, experience) );
                    }

                    foreach (var skill in new[] { Skill.MagicLevel, Skill.Fist, Skill.Club, Skill.Sword, Skill.Axe, Skill.Distance, Skill.Shield, Skill.Fish } )
                    {
                        ulong skillPoints = (ulong)(targetPlayer.Skills.GetSkillPoints(skill) * lossPercent);

                        if (skillPoints > 0)
                        {
                            await Context.AddCommand(new PlayerRemoveSkillPointsCommand(targetPlayer, skill, skillPoints) );
                        }
                    }

                    var corpse = await Context.AddCommand(new TileCreatePlayerCorpseCommand(Creature.Tile, targetPlayer, targetPlayer.Client.GetSkullItem(targetPlayer) == SkullIcon.Red || targetPlayer.Client.GetSkullItem(targetPlayer) == SkullIcon.Black, blesses) );
                        
                             _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromMinutes(5) ) );
                }

                Creature attacker = hits
                    .OrderByDescending(h => h.Value.LastAttack)
                    .Select(h => h.Key)
                    .FirstOrDefault();

                if (attacker is Player attackerPlayer)
                {
                    if (attacker.Tile == null || attacker.IsDestroyed)
                    {

                    }
                    else
                    {
                        if ( !Context.Server.Combats.ContainsDefense(attackerPlayer, targetPlayer) )
                        {
                            Context.AddPacket(attackerPlayer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Warning! The murder of " + targetPlayer.Name + " was not justified.") );
                    
                            await Context.Current.AddCommand(new CreatureAddConditionCommand(attackerPlayer, new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayProtectionZoneBlockSeconds) ) ) );
                        }
                    }
                }
            }

            Context.AddEvent(Creature, new CreatureDeathEventArgs(Creature) );

            await Context.AddCommand(new CreatureDestroyCommand(Creature) );
        }
    }
}