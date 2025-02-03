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

            Creature killer = hits?
                .OrderByDescending(h => h.Value.LastAttack)
                .Select(h => h.Key)
                .FirstOrDefault();

            Creature mostDamage = hits?
                .OrderByDescending(h => h.Value.Damage)
                .Select(h => h.Key)
                .FirstOrDefault();

            if (Creature is Monster monster)
            {
                ulong totalExperience = (ulong)monster.Metadata.Experience;

                ulong totalDamage = (ulong)hits.Values.Sum(h => h.Damage);

                foreach (var pair in hits)
                {
                    Creature attacker = pair.Key;

                    Hit hit = pair.Value;

                    if (attacker is Player player && player.Tile != null && !player.IsDestroyed)
                    {
                        ulong damage = (ulong)hit.Damage;

                        ulong experience = totalExperience * damage / totalDamage;

                        if (experience > 0)
                        {
                            await Context.AddCommand(new PlayerAddExperienceCommand(player, experience) );
                        }
                    }
                }

                var corpse = await Context.AddCommand(new TileCreateMonsterCorpseCommand(monster.Tile, monster.Metadata) );
                    
                         _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromSeconds(30) ) );

                if (corpse is Container container && killer is Player owner && owner.Tile != null && !owner.IsDestroyed)
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
            else if (Creature is Player player)
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

                bool rooking = Context.Server.Config.GameplayRooking.Enabled && player.Vocation != Vocation.None && (player.Experience * (1 - lossPercent) < Context.Server.Config.GameplayRooking.ExperienceThreshold);

                if (rooking)
                {
                    lossPercent = 100 / 100.0;
                }

                ulong experience = (ulong)(player.Experience * lossPercent);

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

                var corpse = await Context.AddCommand(new TileCreatePlayerCorpseCommand(player.Tile, player, rooking || player.Combat.GetSkullIcon(null) == SkullIcon.Red || player.Combat.GetSkullIcon(null) == SkullIcon.Black, blesses) );
                        
                         _ = Context.AddCommand(new ItemDecayDestroyCommand(corpse, TimeSpan.FromMinutes(5) ) );

                if (killer is Player owner)
                {
                    if ( !player.Combat.Attacked(owner) )
                    {
                        if (owner.Tile != null && !owner.IsDestroyed)
                        {
                            Context.AddPacket(owner, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Warning! The murder of " + player.Name + " was not justified.") );

                            await Context.Current.AddCommand(new CreatureAddConditionCommand(owner, new ProtectionZoneBlockCondition(TimeSpan.FromSeconds(Context.Current.Server.Config.GameplayProtectionZoneBlockSeconds) ) ) );
                        }

                        owner.Combat.AddKill(0, player.DatabasePlayerId, true, DateTime.UtcNow);

                        player.Combat.AddDeath(0, owner.DatabasePlayerId, killer.Name, player.Level, true, DateTime.UtcNow);
                    }
                    else
                    {
                        owner.Combat.AddKill(0, owner.DatabasePlayerId, false, DateTime.UtcNow);

                        player.Combat.AddDeath(0, owner.DatabasePlayerId, killer.Name, player.Level, false, DateTime.UtcNow);
                    }
                }
                else
                {
                    player.Combat.AddDeath(0, null, killer == null ? "Environment" : killer.Name, player.Level, false, DateTime.UtcNow);
                }
            }

            Context.AddEvent(Creature, new CreatureDeathEventArgs(Creature, killer, mostDamage) );

            await Context.AddCommand(new CreatureDestroyCommand(Creature) );
        }
    }
}