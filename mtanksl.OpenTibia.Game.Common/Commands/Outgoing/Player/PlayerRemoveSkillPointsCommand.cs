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
            if (SkillPoints > 0)
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

                        if (currentSkillPoints - SkillPoints >= maxSkillPoints)
                        {
                            correctSkillPercent = (byte)Math.Ceiling(100.0 * (currentSkillPoints - SkillPoints - minSkillPoints) / (maxSkillPoints - minSkillPoints));

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

                if (correctSkillLevel < currentSkillLevel)
                {
                    Context.AddEvent(new PlayerAdvanceSkillEventArgs(Player, Skill, currentSkillLevel, correctSkillLevel) );
                }

                switch (Skill)
                {
                    case Skill.MagicLevel:

                        await Context.AddCommand(new PlayerUpdateMagicLevelCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Fist:
                                                            
                        await Context.AddCommand(new PlayerUpdateFistCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Club:

                        await Context.AddCommand(new PlayerUpdateClubCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Sword:

                        await Context.AddCommand(new PlayerUpdateSwordCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Axe:

                        await Context.AddCommand(new PlayerUpdateAxeCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Distance:

                        await Context.AddCommand(new PlayerUpdateDistanceCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Shield:

                        await Context.AddCommand(new PlayerUpdateShieldCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    case Skill.Fish:

                        await Context.AddCommand(new PlayerUpdateFishCommand(Player, currentSkillPoints - SkillPoints, correctSkillLevel, correctSkillPercent) );

                        break;

                    default:

                        throw new NotImplementedException();
                }
            }
        }
    }
}