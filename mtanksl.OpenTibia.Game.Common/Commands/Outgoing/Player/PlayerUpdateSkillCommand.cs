using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerUpdateSkillCommand : Command
    {
        public PlayerUpdateSkillCommand(Player player, Skill skill, ulong skillPoints, byte skillLevel, byte skillPercent)
        {
            Player = player;

            Skill = skill;

            SkillPoints = skillPoints;

            SkillLevel = skillLevel;

            SkillPercent = skillPercent;
        }

        public Player Player { get; }

        public Skill Skill { get; set; }

        public ulong SkillPoints { get; }

        public byte SkillLevel { get; }

        public byte SkillPercent { get; }

        public override Promise Execute()
        {
            if (Player.Skills.GetSkillPoints(Skill) != SkillPoints || Player.Skills.GetSkillLevel(Skill) != SkillLevel || Player.Skills.GetSkillPercent(Skill) != SkillPercent)
            {
                Player.Skills.SetSkillPoints(Skill, SkillPoints);

                if (Player.Skills.GetSkillLevel(Skill) != SkillLevel || Player.Skills.GetSkillPercent(Skill) != SkillPercent)
                {
                    Player.Skills.SetSkillLevel(Skill, SkillLevel);

                    Player.Skills.SetSkillPercent(Skill, SkillPercent);

                    if (Skill == Skill.MagicLevel)
                    {
                        Context.AddPacket(Player, new SendStatusOutgoingPacket(
                            Player.Health, Player.MaxHealth, 
                            Player.Capacity, 
                            Player.Experience, Player.Level, Player.LevelPercent, 
                            Player.Mana, Player.MaxMana, 
                            Player.Skills.GetSkillLevel(Skill.MagicLevel), Player.Skills.GetSkillPercent(Skill.MagicLevel), 
                            Player.Soul, 
                            Player.Stamina) );
                    }
                    else
                    {
                        Context.AddPacket(Player, new SendSkillsOutgoingPacket(
                            Player.Skills.GetSkillLevel(Skill.Fist), Player.Skills.GetSkillPercent(Skill.Fist),
                            Player.Skills.GetSkillLevel(Skill.Club), Player.Skills.GetSkillPercent(Skill.Club),
                            Player.Skills.GetSkillLevel(Skill.Sword), Player.Skills.GetSkillPercent(Skill.Sword),
                            Player.Skills.GetSkillLevel(Skill.Axe), Player.Skills.GetSkillPercent(Skill.Axe),
                            Player.Skills.GetSkillLevel(Skill.Distance), Player.Skills.GetSkillPercent(Skill.Distance),
                            Player.Skills.GetSkillLevel(Skill.Shield), Player.Skills.GetSkillPercent(Skill.Shield),
                            Player.Skills.GetSkillLevel(Skill.Fish), Player.Skills.GetSkillPercent(Skill.Fish) ) );
                    }
                }

                Context.AddEvent(new PlayerUpdateSkillEventArgs(Player, Skill, SkillPoints, SkillLevel, SkillPercent) );
            }

            return Promise.Completed;
        }
    }
}