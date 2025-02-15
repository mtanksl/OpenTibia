﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateSkillCommand : Command
    {
        public PlayerUpdateSkillCommand(Player player, Skill skill, ulong skillPoints, byte skillLevel, byte skillPercent, int conditionSkillLevel, int itemSkillLevel)
        {
            Player = player;

            Skill = skill;

            SkillPoints = skillPoints;

            SkillLevel = skillLevel;

            SkillPercent = skillPercent;

            ConditionSkillLevel = conditionSkillLevel;

            ItemSkillLevel = itemSkillLevel;
        }

        public Player Player { get; }

        public Skill Skill { get; set; }

        public ulong SkillPoints { get; }

        public byte SkillLevel { get; }

        public byte SkillPercent { get; }

        public int ConditionSkillLevel { get; }

        public int ItemSkillLevel { get; }

        public override Promise Execute()
        {
            if (Player.Skills.GetSkillPoints(Skill) != SkillPoints || Player.Skills.GetSkillLevel(Skill) != SkillLevel || Player.Skills.GetSkillPercent(Skill) != SkillPercent || Player.Skills.GetConditionSkillLevel(Skill) != ConditionSkillLevel || Player.Skills.GetItemSkillLevel(Skill) != ItemSkillLevel)
            {
                Player.Skills.SetSkillPoints(Skill, SkillPoints);
                Player.Skills.SetSkillLevel(Skill, SkillLevel);
                Player.Skills.SetSkillPercent(Skill, SkillPercent);
                Player.Skills.SetConditionSkillLevel(Skill, ConditionSkillLevel);
                Player.Skills.SetItemSkillLevel(Skill, ItemSkillLevel);

                if (Skill == Skill.MagicLevel)
                {
                    Context.AddPacket(Player, new SendStatusOutgoingPacket(
                        Player.Health, Player.MaxHealth, 
                        Player.Capacity, 
                        Player.Experience, Player.Level, Player.LevelPercent, 
                        Player.Mana, Player.MaxMana, 
                        Player.Skills.GetClientSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                        Player.Soul, 
                        Player.Stamina) );
                }
                else
                {
                    Context.AddPacket(Player, new SendSkillsOutgoingPacket(
                        Player.Skills.GetClientSkillLevel(Skill.Fist), Player.Skills.GetSkillPercent(Skill.Fist),
                        Player.Skills.GetClientSkillLevel(Skill.Club), Player.Skills.GetSkillPercent(Skill.Club),
                        Player.Skills.GetClientSkillLevel(Skill.Sword), Player.Skills.GetSkillPercent(Skill.Sword),
                        Player.Skills.GetClientSkillLevel(Skill.Axe), Player.Skills.GetSkillPercent(Skill.Axe),
                        Player.Skills.GetClientSkillLevel(Skill.Distance), Player.Skills.GetSkillPercent(Skill.Distance),
                        Player.Skills.GetClientSkillLevel(Skill.Shield), Player.Skills.GetSkillPercent(Skill.Shield),
                        Player.Skills.GetClientSkillLevel(Skill.Fish), Player.Skills.GetSkillPercent(Skill.Fish) ) );
                }

                Context.AddEvent(new PlayerUpdateSkillEventArgs(Player, Skill, SkillPoints, SkillLevel, SkillPercent, ConditionSkillLevel, ItemSkillLevel) );
            }

            return Promise.Completed;
        }
    }
}