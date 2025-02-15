using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerRemoveSkillPointsCommand : Command
    {
        public PlayerRemoveSkillPointsCommand(Player player, Skill skill, ulong skillPoints)
        {
            Player = player;

            Skill = skill;

            SkillPoints = skillPoints;
        }

        public Player Player { get; set; }

        public Skill Skill { get; set; }

        public ulong SkillPoints { get; set; }

        public override async Promise Execute()
        {
            byte currentSkillLevel = Player.Skills.GetSkillLevel(Skill);

            ulong currentSkillPoints = Player.Skills.GetSkillPoints(Skill);

            byte correctSkillLevel;

            byte correctSkillPercent;

            if (currentSkillPoints > SkillPoints)
            {
                correctSkillLevel = currentSkillLevel;

                correctSkillPercent = 0;

                ulong maxSkillPoints = Formula.GetRequiredSkillPoints(Skill, Player.Vocation, (byte)(correctSkillLevel + 1) );

                while (true)
                {
                    ulong minSkillPoints = Formula.GetRequiredSkillPoints(Skill, Player.Vocation, correctSkillLevel);

                    if (currentSkillPoints - SkillPoints >= minSkillPoints)
                    {
                        correctSkillPercent = (byte)Math.Max(0, Math.Min(100, Math.Floor(100.0 * (currentSkillPoints - SkillPoints - minSkillPoints) / (maxSkillPoints - minSkillPoints) ) ) );

                        break;
                    }
                    else
                    {
                        correctSkillLevel--;

                        maxSkillPoints = minSkillPoints;
                    }
                }
            }
            else
            {
                currentSkillPoints = SkillPoints;

                if (Skill == Skill.MagicLevel)
                {
                    correctSkillLevel = 0;
                }
                else
                {
                    correctSkillLevel = 10;
                }

                correctSkillPercent = 0;
            }

            await Context.AddCommand(new PlayerUpdateSkillCommand(Player, Skill, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent, Player.Skills.GetConditionSkillLevel(Skill), Player.Skills.GetItemSkillLevel(Skill) ) );

            if (correctSkillLevel < currentSkillLevel)
            {
                Context.AddEvent(new PlayerAdvanceSkillEventArgs(Player, Skill, currentSkillLevel, correctSkillLevel) );
            }
        }
    }
}